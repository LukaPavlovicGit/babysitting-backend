using BabySitting.Api.Database;
using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Domain.Enums;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

public class AccountCompletion
{
    public sealed record AccountCompletionRequest(
        string CreatedByRole,
        string CreatedByUserId,
        int PostalCode,
        string FirstName,
        string AddressName,
        double AddressLongitude,
        double AddressLatitude,
        List<string> SpeakingLanguages,
        List<string> Skills,
        string Currency,
        double Rate,
        string JobLocation,
        Schedule Schedule,
        int? NumberOfChildren,
        List<string>? ChildrenAgeCategories,
        List<string>? ChildrenCharacteristics,
        string? FamilyDescription)
    {
        public string PhotoUrl { get; init; } = string.Empty;
        public bool SubscribeToJobNotifications { get; init; } = false;
    };

    internal sealed record AccountCompletionResponse(bool IsAccountCompleted);

    public class Command : IRequest<AccountCompletionResponse>
    {
        public Command(AccountCompletionRequest request)
        {
            CreatedByRole = Enum.Parse<RoleEnum>(request.CreatedByRole);
            CreatedByUserId = request.CreatedByUserId;
            PostalCode = request.PostalCode;
            FirstName = request.FirstName;
            AddressName = request.AddressName;
            AddressLongitude = request.AddressLongitude;
            AddressLatitude = request.AddressLatitude;
            SpeakingLanguages = request.SpeakingLanguages.Select(x => Enum.Parse<LanguagesEnum>(x)).ToList();
            Skills = request.Skills.Select(x => Enum.Parse<SkillsEnum>(x)).ToList();
            Currency = Enum.Parse<CurrencyEnum>(request.Currency);
            Rate = request.Rate;
            JobLocation = Enum.Parse<JobLocationEnum>(request.JobLocation);
            Schedule = request.Schedule;
            PhotoUrl = request.PhotoUrl;
            SubscribeToJobNotifications = request.SubscribeToJobNotifications;
            NumberOfChildren = request.NumberOfChildren;
            ChildrenAgeCategories = request.ChildrenAgeCategories?.Select(x => Enum.Parse<ChildrenAgeCategoryEnum>(x)).ToList();
            ChildrenCharacteristics = request.ChildrenCharacteristics?.Select(x => Enum.Parse<ChildrenCharacteristicsEnum>(x)).ToList();
            FamilyDescription = request.FamilyDescription;
        }

        public RoleEnum CreatedByRole { get; set; }
        public string CreatedByUserId { get; set; }
        public int PostalCode { get; set; }
        public string FirstName { get; set; }
        public string AddressName { get; set; }
        public double AddressLongitude { get; set; }
        public double AddressLatitude { get; set; }
        public List<LanguagesEnum> SpeakingLanguages { get; set; }
        public List<SkillsEnum> Skills { get; set; }
        public CurrencyEnum Currency { get; set; }
        public double Rate { get; set; }
        public JobLocationEnum JobLocation { get; set; }
        public Schedule Schedule { get; set; }
        public string PhotoUrl { get; set; }
        public bool SubscribeToJobNotifications { get; set; }
        public int? NumberOfChildren { get; set; }
        public List<ChildrenAgeCategoryEnum>? ChildrenAgeCategories { get; set; }
        public List<ChildrenCharacteristicsEnum>? ChildrenCharacteristics { get; set; }
        public string? FamilyDescription { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
           RuleFor(s => s)
           .Must(MustHaveRequiredParentFields)
           .When(s => s.CreatedByRole == RoleEnum.PARENT)
           .WithMessage("When role is PARENT, NumberOfChildren, ChildrenAgeCategories, ChildrenCharacteristics, and FamilyDescription are required");

            RuleFor(s => s.CreatedByUserId).NotEmpty();
        }
        private bool MustHaveRequiredParentFields(Command request)
        {
            return request.NumberOfChildren != null && request.ChildrenAgeCategories != null && request.ChildrenCharacteristics != null && request.FamilyDescription != null;
        }
    }

    internal sealed class Handler(ApplicationDbContext dbContext, IValidator<AccountCompletion.Command> validator) : IRequestHandler<Command, AccountCompletionResponse>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IValidator<Command> _validator = validator;

        public async Task<AccountCompletionResponse> Handle(Command request, CancellationToken cancellationToken){
            var validationResult = _validator.Validate(request);
            if(!validationResult.IsValid)
            {
                throw new ApplicationException(validationResult.ToString());
            }

            var offer = new Offer(request);
            _dbContext.Add(offer);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            if(result != 2)
            {
                _dbContext.Remove(offer);
                throw new ApplicationException("Failed to save changes");
            }

            var user = await _dbContext.Users.FindAsync(request.CreatedByUserId);
            if (user == null)
            {
                _dbContext.Remove(offer);
                throw new ApplicationException("Failed to save changes on user entity while completing account");
            }

            user.IsAccountCompleted = true;
            user.Role = request.CreatedByRole;
            _dbContext.Update(user);
            
            result = await _dbContext.SaveChangesAsync(cancellationToken);
            if(result != 1)
            {
                _dbContext.Remove(offer);
                throw new ApplicationException("Failed to save changes on user entity while completing account");
            }

            return new AccountCompletionResponse(true);
        }
    }
}

public class AccountCompletionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/complete", async (AccountCompletion.AccountCompletionRequest request, ICurrentUserAccessor currentUser, ISender sender) =>
        {
            var command = new AccountCompletion.Command(request);
            var result = await sender.Send(command);
            return Results.Ok(result);
        });
    }
}
