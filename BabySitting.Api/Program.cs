using BabySitting.Api.Database;
using BabySitting.Api.Entities;
using BabySitting.Api.Extensions;
using BabySitting.Api.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Cache"));

builder.Services.AddAuthorization();

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

//builder.Services.AddIdentityCore<User>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddApiEndpoints();


builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredUniqueChars = 4;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<ISenderEmail, EmailSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

app.Run();

