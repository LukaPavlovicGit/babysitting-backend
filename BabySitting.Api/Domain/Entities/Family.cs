using BabySitting.Api.Domain.Enums;
using MediatR;

namespace BabySitting.Api.Domain.Entities;

public class Family
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public required string Address = string.Empty;

    public required HashSet<LanguagesEnum> SpeakingLanguages { get; set; }

    public required int NumberOfChildren { get; set; }

    public required HashSet<ChildAgeCategoryEnum> ChildrenAgeCategories { get; set; }

    public required String FamilyDescription { get; set; }
}
