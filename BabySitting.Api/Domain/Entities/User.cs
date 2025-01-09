using BabySitting.Api.Domain.Enums;
using BabySitting.Api.Features.Account;
using Microsoft.AspNetCore.Identity;

namespace BabySitting.Api.Domain.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public bool IsAccountCompleted { get; set; } = false;

    public RoleEnum Role { get; set; } = RoleEnum.NONE;

    public double VerificationScore { get; set; } = 0;

    public User(AccountRegistration.Command command)
    {
        Id = Guid.NewGuid().ToString();
        Email = command.Email;
        UserName = command.Email;
        FirstName = command.FirstName;
        LastName = command.LastName;
    }

    public User() { }
}
