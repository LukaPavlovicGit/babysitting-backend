using BabySitting.Api.Database;
using BabySitting.Api.Shared;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using BabySitting.Api.Domain.Entities;

namespace BabySitting.Api.Features.Account;
public class AccountRegistration
{
    public sealed record class AccountRegistrationRequest(string Email, string Password, string FirstName, string LastName);

    internal sealed record class AccountRegistrationResponse(string Email, string FirstName);

    public class Command(AccountRegistrationRequest request) : IRequest<AccountRegistrationResponse>
    {
        public string Email { get; set; } = request.Email;
        public string Password { get; set; } = request.Password;
        public string FirstName { get; set; } = request.FirstName;
        public string LastName { get; set; } = request.LastName;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(s => s.Email).EmailAddress();
            RuleFor(s => s.Password).NotEmpty();
            RuleFor(s => s.FirstName).NotEmpty();
            RuleFor(s => s.LastName).NotEmpty();
        }
    }

    internal sealed class Handler(
        ApplicationDbContext dbContext,
        IValidator<Command> validator,
        UserManager<User> userManager,
        ISenderEmail emailSender,
        IConfiguration configuration
        ) : IRequestHandler<Command, AccountRegistrationResponse>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IValidator<Command> _validator = validator;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ISenderEmail _emailSender = emailSender;
        private readonly IConfiguration _configuration = configuration;

        public async Task<AccountRegistrationResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ApplicationException(validationResult.ToString());
            }

            var user = new User(request);
            _dbContext.Add(user);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new ApplicationException(result.ToString());
            }

            await _emailSender.SendConfirmationEmail(request.Email, user);
            return new AccountRegistrationResponse(request.Email, user.FirstName);
        }
    }
}

public class AccountRegistrationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/register", async (AccountRegistration.AccountRegistrationRequest request, ISender sender) =>
        {
            var command = new AccountRegistration.Command(request);
            var result = await sender.Send(command);
            return Results.Ok(result);
        });
    }
}
