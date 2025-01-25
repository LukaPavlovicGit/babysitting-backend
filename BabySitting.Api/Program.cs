using BabySitting.Api.Database;
using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Extensions;
using BabySitting.Api.Shared;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BabySitting.Api.Domain.Enums;
using Serilog;
using BabySitting.Api.Middleware;
using BabySitting.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        const string DevelopmentCorsPolicy = "DevelopmentCorsPolicy";

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: DevelopmentCorsPolicy,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:3000",
                                                     "http://www.example.com")
                                                    .AllowAnyHeader()
                                                    .AllowAnyMethod();
                              });
        });

        builder.Services.AddSingleton<TokenProvider>();

        builder.Services.AddProblemDetails();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

        builder.Services.AddStackExchangeRedisCache(options =>
            options.Configuration = builder.Configuration.GetConnectionString("Cache"));

        builder.Services.AddAuthorization();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddIdentity<User, IdentityRole>(
            options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredUniqueChars = 4;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

        builder.Services.AddTransient<ISenderEmail, EmailSender>();

        var assembly = typeof(Program).Assembly;

        builder.Services.AddValidatorsFromAssembly(assembly);

        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

        builder.Services.AddCarter();

        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddSignalR()
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.ApplyMigrations();
        }

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in Enum.GetValues<RoleEnum>())
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
        }

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseCors(DevelopmentCorsPolicy);

        app.UseExceptionHandler();

        app.UseMiddleware<RequestContextLoggingMiddleware>();

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.MapCarter();

        app.MapHub<ChatHub>("/chat");

        app.Run();
    }
}