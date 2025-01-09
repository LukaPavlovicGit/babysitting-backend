using BabySitting.Api.Database;
using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Domain.Enums;
using Carter;
using FluentValidation;
using MediatR;

namespace BabySitting.Api.Features.Account;
public class ParentAccountCompletion
{
    public sealed record ParentAccountCompletionRequest(
        Guid UserId,
        string PostalCode,
        string FirstName,
        string AddressName,
        double AddressLongitude,
        double AddressLatitude,
        List<LanguagesEnum> FamilySpeakingLanguages,
        int NumberOfChildren,
        List<ChildAgeCategoryEnum> ChildrenAgeCategories,
        List<ChildCharacteristicsEnum> ChildrenCharacteristics,
        string FamilyDescription,
        List<SkillsEnum> PreferebleSkills,
        CurrencyEnum Currency,
        int Rate,
        JobLocationEnum JobLocation,
        Schedule Schedule)
    {
        public string PhotoUrl { get; init; } = string.Empty;
        public bool SubscribeToJobNotifications { get; init; } = false;
    }

    internal sealed record ParentAccountCompletionResponse(bool IsAccountCompleted);

    public class Command(ParentAccountCompletionRequest request) : IRequest<ParentAccountCompletionResponse>
    {
        public string PhotoUrl { get; set; } = request.PhotoUrl;
        public bool SubscribeToJobNotifications { get; set; } = request.SubscribeToJobNotifications;
        public Guid UserId { get; set; } = request.UserId;
        public string FirstName { get; set; } = request.FirstName;
        public string PostalCode { get; set; } = request.PostalCode;
        public string AddressName { get; set; } = request.AddressName;
        public double AddressLongitude { get; set; } = request.AddressLongitude;
        public double AddressLatitude { get; set; } = request.AddressLatitude;
        public List<LanguagesEnum> FamilySpeakingLanguages { get; set; } = request.FamilySpeakingLanguages;
        public int NumberOfChildren { get; set; } = request.NumberOfChildren;
        public List<ChildAgeCategoryEnum> ChildrenAgeCategories { get; set; } = request.ChildrenAgeCategories;
        public List<ChildCharacteristicsEnum> ChildrenCharacteristics { get; set; } = request.ChildrenCharacteristics;
        public string FamilyDescription { get; set; } = request.FamilyDescription;
        public List<SkillsEnum> PreferebleSkills { get; set; } = request.PreferebleSkills;
        public CurrencyEnum Currency { get; set; } = request.Currency;
        public int Rate { get; set; } = request.Rate;
        public JobLocationEnum JobLocation { get; set; } = request.JobLocation;
        public Schedule Schedule { get; set; } = request.Schedule;
    }

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
        }
    }

    internal sealed class Handler : IRequestHandler<Command, ParentAccountCompletionResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext dbContext, IValidator<Command> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<ParentAccountCompletionResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if(!validationResult.IsValid)
            {
                throw new ApplicationException(validationResult.ToString());
            }

            var parentOffer = new ParentOffer(request);
            _dbContext.Add(parentOffer);

            var result = await _dbContext.SaveChangesAsync();
            if(result != 2)
            {
                throw new ApplicationException("Failed to save changes");
            }

            return new ParentAccountCompletionResponse(true);
        }
    }
}

public class ParentAccountCompletionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/account-complete", async (ParentAccountCompletion.ParentAccountCompletionRequest request, ISender sender) =>
        {
            var command = new ParentAccountCompletion.Command(request);
            var result = await sender.Send(command);
            return Results.Ok(result);
        });
    }
}