using BabySitting.Api.Database;
using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Domain.Enums;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BabySitting.Api.Features.Account;

public class GetAllAccounts
{
    internal sealed record class AccountDetailsDto(string Id, string Email, string FirstName, string LastName, string Role, bool IsAccountCompleted, double VerificationScore);
    internal sealed record class GetAllAccountsResponse(List<AccountDetailsDto> Accounts);

    internal class Query : IRequest<GetAllAccountsResponse> { }

    internal sealed class Handler(ApplicationDbContext dbContext) : IRequestHandler<Query, GetAllAccountsResponse>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<GetAllAccountsResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var accounts = await _dbContext
                .Users
                .Select(u => new AccountDetailsDto(u.Id, u.Email!, u.FirstName, u.LastName, u.Role.ToString(), u.IsAccountCompleted, u.VerificationScore))
                .ToListAsync(cancellationToken);
            return new GetAllAccountsResponse(accounts);
        }
    }
}

public class GetAllOffers
{
    internal sealed record class OfferDetailsDto(
        string Id,
        RoleEnum CreatedByRole,
        string CreatedByUserId,
        string FirstName,
        int PostalCode,
        string Addressname,
        double AddressLongitude,
        double AddressLatitude,
        List<LanguagesEnum> SpeakingLanguages,
        List<SkillsEnum> Skills,
        CurrencyEnum Currency,
        double Rate,
        JobLocationEnum JobLocation,
        Schedule Schedule,
        int? NumberOfChildren,
        List<ChildrenAgeCategoryEnum>? ChildrenAgeCategories,
        List<ChildrenCharacteristicsEnum>? ChildrenCharacteristics,
        string? FamilyDescription
    );

    internal sealed record class GetAllOffersResponse(List<OfferDetailsDto> Offers);

    internal class Query : IRequest<GetAllOffersResponse> { }

    internal sealed class Handler(ApplicationDbContext dbContext) : IRequestHandler<Query, GetAllOffersResponse>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<GetAllOffersResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var offers = await _dbContext
                .Offers
                .Select(o => new OfferDetailsDto(
                    o.Id.ToString(),
                    o.CreatedByRole,
                    o.CreatedByUserId,
                    o.FirstName,
                    o.PostalCode,
                    o.AddressName,
                    o.AddressLongitude,
                    o.AddressLatitude,
                    o.SpeakingLanguages,
                    o.Skills,
                    o.Currency,
                    o.Rate,
                    o.JobLocation,
                    o.Schedule,
                    o.NumberOfChildren,
                    o.ChildrenAgeCategories,
                    o.ChildrenCharacteristics,
                    o.FamilyDescription
                ))
                .ToListAsync(cancellationToken);
            return new GetAllOffersResponse(offers);
        }
    }
}

public class GetAllAccountsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/get-data", async (ISender sender) =>
        {
            var accountsQuery = new GetAllAccounts.Query();
            var accountsResponse = await sender.Send(accountsQuery);

            var offersQuery = new GetAllOffers.Query();
            var offersResponse = await sender.Send(offersQuery);

            return Results.Ok(new { accountsResponse.Accounts, offersResponse.Offers });
        });
    }
}
