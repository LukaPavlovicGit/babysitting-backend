using BabySitting.Api.Database;
using BabySitting.Api.Domain.Enums;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BabySitting.Api.Features;

public class GetAccountsByCriteria
{
    public sealed record class GetAccountsByCriteriaRequest(bool? IsAccountCompleted, string? Email, string? FirstName, string? LastName, string? Role);

    public sealed record class AccountDetailsDto(string Id, string Email, string FirstName, string LastName, string Role, bool IsAccountCompleted, double VerificationScore);
    
    public sealed record class GetAccountByCriteriaResponse(List<AccountDetailsDto> Accounts);

    public class Command(GetAccountsByCriteriaRequest request) : IRequest<GetAccountByCriteriaResponse>
    {
        public bool? IsAccountCompleted { get; set; } = request.IsAccountCompleted;
        public string? Email { get; set; } = request.Email;
        public string? FirstName { get; set; } = request.FirstName;
        public string? LastName { get; set; } = request.LastName;
        public string? Role { get; set; } = request.Role;
    }

    public sealed class Handler(ApplicationDbContext dbContext) : IRequestHandler<Command, GetAccountByCriteriaResponse>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<GetAccountByCriteriaResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            
            var query = _dbContext.Users.AsQueryable();

            if (request.IsAccountCompleted != null)
            {
                query = query.Where(u => u.IsAccountCompleted == request.IsAccountCompleted);
            }
            if (request.Email != null)
            {
                query = query.Where(u => u.Email != null && request.Email.ToLower() == u.Email.ToLower());
            }
            if (request.FirstName != null)
            {
                query = query.Where(u => u.FirstName != null && request.FirstName.ToLower() == u.FirstName.ToLower());
            }
            if(request.LastName != null)
            {
                query = query.Where(u => u.LastName != null && request.LastName.ToLower() == u.LastName.ToLower());
            }
            if(request.Role != null)
            {
                query = query.Where(u => u.Role == Enum.Parse<RoleEnum>(request.Role));
            }

            var accounts = await query.ToListAsync(cancellationToken);
            if(accounts == null)
            {
                throw new ApplicationException("GetAccountsByCriteria.Handler.Handle.(accounts == null)");
            }

            return new GetAccountByCriteriaResponse(accounts.Select(a => new AccountDetailsDto(a.Id, a.Email ?? "", a.FirstName, a.LastName, a.Role.ToString(), a.IsAccountCompleted, a.VerificationScore)).ToList());
        }
    }
}

public class GetAccountsByCriteriaEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/get-by-criteria", async (GetAccountsByCriteria.GetAccountsByCriteriaRequest request, ISender sender) =>
        {
            var command = new GetAccountsByCriteria.Command(request);
            var result = await sender.Send(command);
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}