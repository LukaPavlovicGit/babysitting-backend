using BabySitting.Api.Database;
using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Domain.Enums;
using Carter;
using FluentValidation;
using MediatR;

public class BabysitterAccountCompletion
{
    public sealed record BabysitterAccountCompletionRequest(
        string PostalCode,
        string FirstName,
        string AddressName,
        double AddressLongitude,
        double AddressLatitude,
        List<LanguagesEnum> SpeakingLanguages,
        List<SkillsEnum> Skills,
        CurrencyEnum Currency,
        int Rate,
        JobLocationEnum JobLocation,
        Schedule Schedule)
    {
        public string PhotoUrl { get; init; } = string.Empty;
        public bool SubscribeToJobNotifications { get; init; } = false;
    }

    internal sealed record BabysitterAccountCompletionResponse(bool IsAccountCompleted);

    public class Command(BabysitterAccountCompletionRequest request, string userId) : IRequest<BabysitterAccountCompletionResponse>
    {
        public string UserId { get; set; } = userId;
        public string PhotoUrl { get; set; } = request.PhotoUrl;
        public bool SubscribeToJobNotifications { get; set; } = request.SubscribeToJobNotifications;
        public string PostalCode { get; set; } = request.PostalCode;
        public string FirstName { get; set; } = request.FirstName;
        public string AddressName { get; set; } = request.AddressName;
        public double AddressLongitude { get; set; } = request.AddressLongitude;
        public double AddressLatitude { get; set; } = request.AddressLatitude;
        public List<LanguagesEnum> SpeakingLanguages { get; set; } = request.SpeakingLanguages;
        public List<SkillsEnum> Skills { get; set; } = request.Skills;
        public CurrencyEnum Currency { get; set; } = request.Currency;
        public int Rate { get; set; } = request.Rate;
        public JobLocationEnum JobLocation { get; set; } = request.JobLocation;
        public Schedule Schedule { get; set; } = request.Schedule;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
        }
    }

    internal sealed class Handler : IRequestHandler<Command, BabysitterAccountCompletionResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext dbContext, IValidator<Command> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<BabysitterAccountCompletionResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if(!validationResult.IsValid)
            {
                throw new ApplicationException(validationResult.ToString());
            }

            var babysitterOffer = new BabysitterOffer(request);
            _dbContext.Add(babysitterOffer);

            var result = await _dbContext.SaveChangesAsync();
            if(result != 2)
            {
                throw new ApplicationException("Failed to save changes");
            }

            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
            {
                throw new ApplicationException("Failed to save changes on user entity while completing babysitter account");
            }

            user.Role = RoleEnum.BABYSITTER;
            user.IsAccountCompleted = true;

            result = await _dbContext.SaveChangesAsync();
            if(result != 1)
            {
                _dbContext.Remove(babysitterOffer);
                throw new ApplicationException("Failed to save changes on user entity while completing babysitter account");
            }

            return new BabysitterAccountCompletionResponse(true);
        }
    }
}

public class BabysitterAccountCompletionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/babysitter-account-complete", async (BabysitterAccountCompletion.BabysitterAccountCompletionRequest request, ICurrentUserAccessor currentUser, ISender sender) =>
        {
            var command = new BabysitterAccountCompletion.Command(request, currentUser.User.Id);
            var result = await sender.Send(command);
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}

