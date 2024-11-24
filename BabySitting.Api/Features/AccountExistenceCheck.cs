using BabySitting.Api.Contracts;
using BabySitting.Api.Database;
using BabySitting.Api.Shared;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BabySitting.Api.Features;

public class AccountExistenceCheck
{
    public class Query : IRequest<Result<AccountExistenceCheckResponse>>
    {
        public string Email { get; set; } = string.Empty;
    }

    internal sealed class Handler : IRequestHandler<Query, Result<AccountExistenceCheckResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<AccountExistenceCheckResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _dbContext
                .Users
                .AsNoTracking()
                .Where(u => string.Equals(u.Email, request.Email, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return Result.Failure<AccountExistenceCheckResponse>(new Error(
                    "GetUser.Null",
                    "The user with the specified EMAIL was not found"
                ));
            }

            return new AccountExistenceCheckResponse(user.Id);
        }
    }
}

public class CheckUserExistsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/account/checkAccountExists/{email}", async (string email, ISender sender) =>
        {
            var query = new AccountExistenceCheck.Query { Email = email };
            var result = await sender.Send(query);
            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}