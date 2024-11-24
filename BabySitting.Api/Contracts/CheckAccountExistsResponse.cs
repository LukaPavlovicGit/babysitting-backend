using System;

namespace BabySitting.Api.Contracts;
public class CheckAccountExistsResponse
{
    public CheckAccountExistsResponse() 
    {
        
    }

    public CheckAccountExistsResponse(string id)
    {
        UserId = Guid.Parse(id);
    }

    public Guid UserId { get; set; }
}
