using System;

namespace BabySitting.Api.Contracts.responses;
public class AccountExistenceCheckResponse
{
    public AccountExistenceCheckResponse() { }

    public AccountExistenceCheckResponse(string id)
    {
        UserId = Guid.Parse(id);
    }

    public Guid UserId { get; set; }
}
