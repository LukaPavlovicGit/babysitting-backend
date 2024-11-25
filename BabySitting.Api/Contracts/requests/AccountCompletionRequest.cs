using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Domain.Enums;

namespace BabySitting.Api.Contracts.requests;
public class AccountCompletionRequest
{
    public RoleEnum Role { get; set; } = RoleEnum.NONE;

    public required FamilyInformation FamilyInformation { get; set; }

    public required Job JobOffer { get; set; }

    public String PhotoUrl { get; set; } = string.Empty;

    public Boolean SubscribeToJobNotifications { get; set; } = false;
}