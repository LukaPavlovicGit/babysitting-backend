using BabySitting.Api.Database;
using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Infrastructure;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BabySitting.Api.Features.Account;

public class AccountLogin
{
    internal sealed class AccountLoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }

    internal record AccountLoginResponse(string Token, bool IsAccountCompleted);
    
    internal sealed class Query(AccountLoginRequest request) : IRequest<AccountLoginResponse>
    {
        public string Email { get; set; } = request.Email;
        public string Password { get; set; } = request.Password;
        public bool RememberMe { get; set; } = request.RememberMe;
    }

    internal sealed class Handler(ApplicationDbContext dbContext, SignInManager<User> signInManager, TokenProvider tokenProvider) : IRequestHandler<Query, AccountLoginResponse>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly TokenProvider _tokenProvider = tokenProvider;

        public async Task<AccountLoginResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new ApplicationException(result.ToString());
            }
            // TODO: Handle two-factor authentication case
            if (result.RequiresTwoFactor)
            {
                throw new ApplicationException(result.ToString());// Handle two-factor authentication case
            }
            // TODO: Handle lockout scenario
            if (result.IsLockedOut)
            {
                throw new ApplicationException(result.ToString());// Handle lockout scenario
            }

            var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);
            string token = _tokenProvider.Create(user!);
            return new AccountLoginResponse(token, user!.IsAccountCompleted);
        }
    }
}

public class AccountLoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/login", async (AccountLogin.AccountLoginRequest request, ISender sender) =>
        {
            var command = new AccountLogin.Query(request);
            var result = await sender.Send(command);
            return Results.Ok(result);
        });
    }
}
