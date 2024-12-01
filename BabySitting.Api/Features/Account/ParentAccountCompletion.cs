using BabySitting.Api.Contracts.requests;
using BabySitting.Api.Database;
using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Domain.Enums;
using BabySitting.Api.Shared;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;

namespace BabySitting.Api.Features.Account;
public class ParentAccountCompletion
{
    public class Command : IRequest<Result<string>>
    {
        public string PhotoUrl { get; set; } = string.Empty;
        public bool SubscribeToJobNotifications { get; set; } = false;
        public required Guid UserId { get; set; }
        public required string FirstName { get; set; }
        public required string AddressName { get; set; }
        public required double AddressLongitude { get; set; }
        public required double AddressLatitude { get; set; }
        public required List<LanguagesEnum> FamilySpeakingLanguages { get; set; }
        public required int NumberOfChildren { get; set; }
        public required List<ChildAgeCategoryEnum> ChildrenAgeCategories { get; set; }
        public required List<ChildCharacteristicsEnum> ChildrenCharacteristics { get; set; }
        public required string FamilyDescription { get; set; }
        public required List<SkillsEnum> PreferebleSkills { get; set; }
        public required CurrencyEnum Currency { get; set; }
        public required int Rate { get; set; }
        public required JobLocationEnum JobLocation { get; set; }
        public required Schedule Schedule { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
           
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<string>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext dbContext, IValidator<Command> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if(!validationResult.IsValid)
            {
                return Result.Failure<string>(new Error("AccountRegistrationRequest.validation", validationResult.ToString()));
            }

            var parentOffer = new ParentOffer
            {
                UserId = request.UserId,
                FirstName = request.FirstName,
                AddressName = request.AddressName,
                AddressLatitude = request.AddressLatitude,
                AddressLongitude = request.AddressLongitude,
                FamilySpeakingLanguages = request.FamilySpeakingLanguages,
                NumberOfChildren = request.NumberOfChildren,
                ChildrenAgeCategories = request.ChildrenAgeCategories,
                ChildrenCharacteristics = request.ChildrenCharacteristics,
                FamilyDescription = request.FamilyDescription,
                PreferebleSkills = request.PreferebleSkills,
                Currency = request.Currency,
                Rate = request.Rate,
                JobLocation = request.JobLocation,
                Schedule = request.Schedule,
            };


            _dbContext.Add(parentOffer);

            var result = await _dbContext.SaveChangesAsync();

            if(result == 2)
            {
                return Result.Success("Successfully");
            }

            return Result.Failure<string>(new Error("ParentAccountCompletionRequest.create", ""));
        }
    }
}

public class ParentAccountCompletionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/account-complete", async (ParentAccountCompletionRequest request, ISender sender) =>
        {
            var command = request.Adapt<ParentAccountCompletion.Command>();
            var result = await sender.Send(command);
            if(result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}