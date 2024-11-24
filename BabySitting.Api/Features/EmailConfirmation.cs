using BabySitting.Api.Database;
using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Shared;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace BabySitting.Api.Features;
public class EmailConfirmation
{
    public class Command : IRequest<Result<string>>
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }


    internal sealed class Handler : IRequestHandler<Command, Result<string>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<Handler> _logger;

        public Handler(ApplicationDbContext dbContext, UserManager<User> userManager, ILogger<Handler> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.UserId == null || request.Token == null)
            {
                return Result.Failure<string>(new Error("EmailConfirmationRequest", "UserId or Token is null"));
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure<string>(new Error("EmailConfirmationRequest", "User not found by UserId"));
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (result.Succeeded)
            {
                return Result.Success("Thank you for confirming your email");
            }

            return Result.Failure<string>(new Error("EmailConfirmationRequest", $"Email cannot be confirmed: {result.ToString()}"));
        }
    }
}


public class EmailCOnfirmationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/account/emailConfirmation", async (string userId, string token, ISender sender) =>
        {
            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);
            var command = new EmailConfirmation.Command { UserId = userId, Token = decodedToken };
            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }

}