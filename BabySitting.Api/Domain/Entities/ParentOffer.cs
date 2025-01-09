using BabySitting.Api.Domain.Enums;
using BabySitting.Api.Features.Account;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabySitting.Api.Domain.Entities;

[Table("ParentOffers")]
public class ParentOffer
{
    [Key]
    public int Id { get; set; }

    public Guid UserId { get; set; } = Guid.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string AddressName { get; set; } = string.Empty;
    
    public double AddressLongitude { get; set; } = 0;
    
    public double AddressLatitude { get; set; } = 0;

    public List<LanguagesEnum> FamilySpeakingLanguages { get; set; } = [];

    public int NumberOfChildren { get; set; }

    public List<ChildAgeCategoryEnum> ChildrenAgeCategories { get; set; } = [];

    public List<ChildCharacteristicsEnum> ChildrenCharacteristics { get; set; } = [];

    public string FamilyDescription {  get; set; } = string.Empty;

    public List<SkillsEnum> PreferebleSkills { get; set; } = [];

    public CurrencyEnum Currency { get; set; } = CurrencyEnum.RSD;

    public int Rate { get; set; } 

    public JobLocationEnum JobLocation { get; set; } = JobLocationEnum.AT_PARENT_HOME;

    public Schedule Schedule { get; set; } = new Schedule();

    public ParentOffer(ParentAccountCompletion.Command request)
    {
        UserId = request.UserId!;
        FirstName = request.FirstName;
        PostalCode = request.PostalCode;
        AddressName = request.AddressName;
        AddressLatitude = request.AddressLatitude;
        AddressLongitude = request.AddressLongitude;
        FamilySpeakingLanguages = request.FamilySpeakingLanguages;
        NumberOfChildren = request.NumberOfChildren;
        ChildrenAgeCategories = request.ChildrenAgeCategories;
        ChildrenCharacteristics = request.ChildrenCharacteristics;
        FamilyDescription = request.FamilyDescription;
        PreferebleSkills = request.PreferebleSkills;
        Currency = request.Currency;
        Rate = request.Rate;
        JobLocation = request.JobLocation;
        Schedule = request.Schedule;
    }

    public ParentOffer() { }

}
