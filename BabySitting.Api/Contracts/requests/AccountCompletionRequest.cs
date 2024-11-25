using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Domain.Enums;

namespace BabySitting.Api.Contracts.requests;

public class AccountCompletionRequest
{
    public RoleEnum Role { get; set; }

    public string PhotoUrl { get; set; } = string.Empty;

    public bool SubscribeToJobNotifications { get; set; } = false;

    public OfferCreateDto? OfferCreateDto { get; set; }

    public FamilyCreateDto? FamilyInformationDto { get; set; }

}

public class FamilyCreateDto
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;
    
    public required string Address { get; set; }

    public required HashSet<LanguagesEnum> SpeakingLanguages { get; set; }

    public required int NumberOfChildren { get; set; }

    public required HashSet<ChildAgeCategoryEnum> ChildrenAgeCategories { get; set; }

    public required string FamilyDescription { get; set; }

}

public class OfferCreateDto
{
    public Guid UserId { get; set; }

    public HashSet<SkillsEnum> PreferebleSkills { get; set; } = new HashSet<SkillsEnum>();

    public CurrencyEnum Currency { get; set; }

    public int Rate { get; set; }

    public JobLocationEnum JobLocation { get; set; }

    public required Schedule Schedule { get; set; }
}

