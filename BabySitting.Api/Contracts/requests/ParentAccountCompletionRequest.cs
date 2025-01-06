using BabySitting.Api.Domain.Entities;
using BabySitting.Api.Domain.Enums;

namespace BabySitting.Api.Contracts.requests;

public sealed class ParentAccountCompletionRequest
{
    public string PhotoUrl { get; set; } = string.Empty;

    public bool SubscribeToJobNotifications { get; set; } = false;

    public required Guid UserId { get; set; }

    public required string PostalCode { get; set; }

    public required string FirstName { get; set; }

    public required string AddressName { get; set; }

    public required double AddressLongitude { get; set; }

    public required double AddressLatitude { get; set; }

    public required List<LanguagesEnum> FamilySpeakingLanguages { get; set; }

    public required int NumberOfChildren { get; set; }

    public required List<ChildAgeCategoryEnum> ChildrenAgeCategories { get; set; }

    public required List<ChildCharacteristicsEnum> ChildrenCharacteristics { get; set; }

    public required string FamilyDescription { get; set; }

    public required List<SkillsEnum> PreferebleSkills { get; set; }

    public required CurrencyEnum Currency { get; set; }

    public required int Rate { get; set; }

    public required JobLocationEnum JobLocation { get; set; }

    public required Schedule Schedule { get; set; }

}
