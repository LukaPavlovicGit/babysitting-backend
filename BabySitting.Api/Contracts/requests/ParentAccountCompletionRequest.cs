using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Domain.Enums;

namespace BabySitting.Api.Contracts.requests;

public class ParentAccountCompletionRequest
{
    public string PhotoUrl { get; set; } = string.Empty;

    public bool SubscribeToJobNotifications { get; set; } = false;

    public required ParentOfferCreateDto? ParentOfferCreateDto { get; set; }

}

public class ParentOfferCreateDto
{
    public required Guid UserId { get; set; }

    public required string FirstName{ get; set; }

    public required string FamilyAddress { get; set; }

    public required HashSet<LanguagesEnum> FamilySpeakingLanguages { get; set; }

    public required int NumberOfChildren { get; set; }

    public required HashSet<ChildAgeCategoryEnum> ChildrenAgeCategories { get; set; }

    public required HashSet<ChildCharacteristicsEnum> ChildrenCharacteristics { get; set; }

    public required string FamilyDescription { get; set; }

    public required HashSet<SkillsEnum> PreferebleSkills { get; set; }

    public required CurrencyEnum Currency { get; set; }

    public required int Rate { get; set; }

    public required JobLocationEnum JobLocation { get; set; }

    public required Schedule Schedule { get; set; }

}

