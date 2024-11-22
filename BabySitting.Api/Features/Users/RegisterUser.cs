using BabySitting.Api.Contracts;
using BabySitting.Api.Database;
using BabySitting.Api.Shared;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using BabySitting.Api.Domain.Entities;

namespace BabySitting.Api.Features.Users;
public static class RegisterUser
{

    public class Command : IRequest<Result<Guid>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
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


    internal sealed class Handler : IRequestHandler<Command, Result<Guid>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<Command> _validator;
        private readonly UserManager<User> _userManager;
        private readonly ISenderEmail _emailSender;
        private readonly IConfiguration _configuration;

        public Handler(
            ApplicationDbContext dbContext,
            IValidator<Command> validator,
            UserManager<User> userManager,
            ISenderEmail emailSender,
            IConfiguration configuration
        )
        {
            this._dbContext = dbContext;
            this._validator = validator;
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._configuration = configuration;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {

            var validationResult = _validator.Validate(request);

            if(!validationResult.IsValid)
            {
                return Result.Failure<Guid>(new Error("RegisterUserRequest.validation", validationResult.ToString()));
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            _dbContext.Add(user);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _emailSender.SendConfirmationEmail(request.Email, user);
                return Result.Success<Guid>(Guid.Parse(user.Id));
            }

            return Result.Failure<Guid>(new Error("RegisterUserRequest.create", result.ToString()));
        }

    }
}

public class RegisterUserEndpoint : ICarterModule
{ 
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/register", async (RegisterUserRequest request, ISender sender) => 
        {

            var command = request.Adapt<RegisterUser.Command>();
            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }

}
