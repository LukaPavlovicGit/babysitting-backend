using BabySitting.Api.Database;
using BabySitting.Api.Domain.Entities;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace BabySitting.Api.Features.Account;
public class AccountActivation
{
    internal sealed record AccountActivationResponse(bool IsSuccess, string Message);

    internal class Command : IRequest<AccountActivationResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

    internal sealed class Handler(
        ApplicationDbContext dbContext,
        UserManager<User> userManager,
        ILogger<Handler> logger
    ) : IRequestHandler<Command, AccountActivationResponse>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ILogger<Handler> _logger = logger;

        public async Task<AccountActivationResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.UserId == null || request.Token == null)
            {
                throw new ApplicationException("EmailConfirmationRequest: UserId or Token is null");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ApplicationException("EmailConfirmationRequest: User not found by UserId");
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"EmailConfirmationRequest: Email cannot be confirmed: {result.ToString()}");
            }

            return new AccountActivationResponse(true, "Thank you for confirming your email");
        }
    }
}

public class EmailCOnfirmationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/account/email-confirmation", async (string userId, string token, ISender sender) =>
        {
            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);
            var command = new AccountActivation.Command { UserId = userId, Token = decodedToken };
            var result = await sender.Send(command);
            return Results.Ok(result);
        });
    }

}