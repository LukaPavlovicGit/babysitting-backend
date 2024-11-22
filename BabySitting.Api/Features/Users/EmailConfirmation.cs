using BabySitting.Api.Database;
using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Shared;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BabySitting.Api.Features.Users;
public class EmailConfirmation
{
    public class Command : IRequest<Result<String>>
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }


    internal sealed class Handler : IRequestHandler<Command, Result<String>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public Handler(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<Result<String>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.UserId == null || request.Token == null)
            {
                return Result.Failure<String>(new Error("EmailConfirmationRequest", "UserId or Token is null"));
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if(user == null)
            {
                return Result.Failure<String>(new Error("EmailConfirmationRequest", "User not found by UserId"));
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (result.Succeeded)
            {
                return Result.Success<String>("Thank you for confirming your email");
            }

            return Result.Failure<String>(new Error("EmailConfirmationRequest", "Email cannot be confirmed"));
        }
    }
}


public class EmailCOnfirmationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/emailConfirmation", async (string userId, string token, ISender sender) =>
        {
            var command = new EmailConfirmation.Command{ UserId = userId, Token = token };
            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
     
}