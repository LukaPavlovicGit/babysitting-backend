﻿using BabySitting.Api.Contracts.requests;
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
    public class Command(ParentAccountCompletionRequest request) : IRequest<Result<string>>
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

            var parentOffer = new ParentOffer(request);


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
            var command = new ParentAccountCompletion.Command(request);
            var result = await sender.Send(command);
            if(result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}