using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BabySitting.Api.Domain.Enums;

namespace BabySitting.Api.Domain.Entities;

[Table("Offers")]
public class Offer
{
    [Key]
    public int Id { get; set; }
    public RoleEnum CreatedByRole { get; set; } = RoleEnum.NONE;
    public string CreatedByUserId { get; set; } = string.Empty;
    public string AcceptedByUserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string AddressName { get; set; } = string.Empty;
    public double AddressLongitude { get; set; } = 0;
    public double AddressLatitude { get; set; } = 0;
    public List<LanguagesEnum> SpeakingLanguages { get; set; } = [];
    public List<SkillsEnum> Skills { get; set; } = [];
    public CurrencyEnum Currency { get; set; } = CurrencyEnum.RSD;
    public int Rate { get; set; } = 0;
    public JobLocationEnum JobLocation { get; set; } = JobLocationEnum.AT_PARENT_HOME;
    public Schedule Schedule { get; set; } = new Schedule();
    public string PhotoUrl { get; set; } = string.Empty;
    public bool SubscribeToJobNotifications { get; set; } = false;
    public int? NumberOfChildren { get; set; } = -1;
    public List<ChildAgeCategoryEnum>? ChildrenAgeCategories { get; set; } = [];
    public List<ChildCharacteristicsEnum>? ChildrenCharacteristics { get; set; } = [];
    public string? FamilyDescription { get; set; } = string.Empty;

    public Offer(AccountCompletion.Command request)
    {
        CreatedByRole = request.CreatedByRole;
        CreatedByUserId = request.CreatedByUserId;
        FirstName = request.FirstName;
        PostalCode = request.PostalCode;
        AddressName = request.AddressName;
        AddressLongitude = request.AddressLongitude;
        AddressLatitude = request.AddressLatitude;
        SpeakingLanguages = request.SpeakingLanguages;
        Skills = request.Skills;
        Currency = request.Currency;
        Rate = request.Rate;
        JobLocation = request.JobLocation;
        Schedule = request.Schedule;
        PhotoUrl = request.PhotoUrl;
        SubscribeToJobNotifications = request.SubscribeToJobNotifications;
        NumberOfChildren = request.NumberOfChildren;
        ChildrenAgeCategories = request.ChildrenAgeCategories;
        ChildrenCharacteristics = request.ChildrenCharacteristics;
        FamilyDescription = request.FamilyDescription;
    }

    public Offer() { }
}
