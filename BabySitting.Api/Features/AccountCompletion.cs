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
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        RoleEnum CreatedByRole,
        
        string CreatedByUserId,
        
        int PostalCode,
        
        string FirstName,
        
        string AddressName,
        
        double AddressLongitude,

        double AddressLatitude,

        List<LanguagesEnum> SpeakingLanguages,
        
        //[property: JsonConverter(typeof(JsonStringEnumConverter))]
        List<SkillsEnum> Skills,
        
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        CurrencyEnum Currency,
        
        double Rate,
        
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        JobLocationEnum JobLocation,
        
        Schedule Schedule,
        int? NumberOfChildren,
        
        //[property: JsonConverter(typeof(JsonStringEnumConverter))]
        List<ChildrenAgeCategoryEnum>? ChildrenAgeCategories,
        
        //[property: JsonConverter(typeof(JsonStringEnumConverter))]
        List<ChildrenCharacteristicsEnum>? ChildrenCharacteristics,
        
        string? FamilyDescription)
    {
        public string PhotoUrl { get; init; } = string.Empty;
        public bool SubscribeToJobNotifications { get; init; } = false;
    };

    internal sealed record AccountCompletionResponse(bool IsAccountCompleted);

    public class Command(AccountCompletionRequest request) : IRequest<AccountCompletionResponse>
    {
        public RoleEnum CreatedByRole { get; set; } = request.CreatedByRole;
        public string CreatedByUserId { get; set; } = request.CreatedByUserId;
        public int PostalCode { get; set; } = request.PostalCode;
        public string FirstName { get; set; } = request.FirstName;
        public string AddressName { get; set; } = request.AddressName;
        public double AddressLongitude { get; set; } = request.AddressLongitude;
        public double AddressLatitude { get; set; } = request.AddressLatitude;
        public List<LanguagesEnum> SpeakingLanguages { get; set; } = request.SpeakingLanguages;
        public List<SkillsEnum> Skills { get; set; } = request.Skills;
        public CurrencyEnum Currency { get; set; } = request.Currency;
        public double Rate { get; set; } = request.Rate;
        public JobLocationEnum JobLocation { get; set; } = request.JobLocation;
        public Schedule Schedule { get; set; } = request.Schedule;
        public string PhotoUrl { get; set; } = request.PhotoUrl;
        public bool SubscribeToJobNotifications { get; set; } = request.SubscribeToJobNotifications;
        public int? NumberOfChildren { get; set; } = request.NumberOfChildren;
        public List<ChildrenAgeCategoryEnum>? ChildrenAgeCategories { get; set; } = request.ChildrenAgeCategories;
        public List<ChildrenCharacteristicsEnum>? ChildrenCharacteristics { get; set; } = request.ChildrenCharacteristics;
        public string? FamilyDescription { get; set; } = request.FamilyDescription;
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
