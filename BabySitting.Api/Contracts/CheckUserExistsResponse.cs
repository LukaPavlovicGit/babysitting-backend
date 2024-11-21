using System;

namespace BabySitting.Api.Contracts;
public class CheckUserExistsResponse
{
    public CheckUserExistsResponse() 
    {
        
    }

    public CheckUserExistsResponse(string id)
    {
        UserId = Guid.Parse(id);
    }

    public Guid UserId { get; set; }
}
