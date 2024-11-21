using BabySitting.Api.Contracts;
using BabySitting.Api.Database;
using BabySitting.Api.Shared;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BabySitting.Api.Features.Users;

public class CheckUserExists
{
    public class Query : IRequest<Result<CheckUserExistsResponse>>
    {
        public string Email { get; set; } = string.Empty;
    }

    internal sealed class Handler : IRequestHandler<Query, Result<CheckUserExistsResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<CheckUserExistsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _dbContext
                .Users
                .AsNoTracking()
                .Where(u => string.Equals(u.Email, request.Email, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null) {
                return Result.Failure<CheckUserExistsResponse>(new Error(
                    "GetUser.Null",
                    "The user with the specified EMAIL was not found"
                ));
            }

            return new CheckUserExistsResponse(user.Id);
        }
    }
}

public class CheckUserExistsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("CheckUserExists/{email}", async (string email, ISender sender) =>
        {
            var query = new CheckUserExists.Query { Email = email };
            var result = await sender.Send(query);
            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }
            return Results.Ok(result.Value);
        }); 
    }
}