using BabySitting.Api.Database;
using BabySitting.Api.Shared;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BabySitting.Api.Features.Account;

public class GetAccountByEmail
{   
    internal record AccountDetailsResponse(string UserId, string Email, string FirstName, string LastName);
    
    internal class Query : IRequest<Result<AccountDetailsResponse>>
    {
        public string Email { get; set; } = string.Empty;
    }

    internal sealed class Handler(ApplicationDbContext dbContext) : IRequestHandler<Query, Result<AccountDetailsResponse>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Result<AccountDetailsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _dbContext
                .Users
                .AsNoTracking()
                .Where(u => u.Email!.ToLower() == request.Email.ToLower())
                .FirstOrDefaultAsync(cancellationToken) 
                ?? throw new ApplicationException("User not found");
                
            return new AccountDetailsResponse(user.Id, user.Email!, user.FirstName, user.LastName);
        }
    }
}

public class CheckUserExistsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/account/get-account-by-email/{email}", async (string email, ISender sender) =>
        {
            var query = new GetAccountByEmail.Query { Email = email };
            var result = await sender.Send(query);
            return Results.Ok(result.Value);
        });
    }
}