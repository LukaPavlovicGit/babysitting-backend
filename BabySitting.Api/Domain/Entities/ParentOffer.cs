using BabySitting.Api.Domain.Enums;
using BabySitting.Api.Features.Account;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabySitting.Api.Domain.Entities;

[Table("ParentOffers")]
public class ParentOffer(ParentAccountCompletion.Command request)
{
    [Key]
    public int Id { get; set; }

    public Guid UserId { get; set; } = request.UserId;

    public string FirstName { get; set; } = request.FirstName;

    public string PostalCode { get; set; } = request.PostalCode;

    public string AddressName { get; set; } = request.AddressName;

    public double AddressLongitude { get; set; } = request.AddressLongitude;

    public double AddressLatitude { get; set; } = request.AddressLatitude;

    public List<LanguagesEnum> FamilySpeakingLanguages { get; set; } = request.FamilySpeakingLanguages;

    public int NumberOfChildren { get; set; } = request.NumberOfChildren;

    public List<ChildAgeCategoryEnum> ChildrenAgeCategories { get; set; } = request.ChildrenAgeCategories;

    public List<ChildCharacteristicsEnum> ChildrenCharacteristics { get; set; } = request.ChildrenCharacteristics;

    public string FamilyDescription { get; set; } = request.FamilyDescription;

    public List<SkillsEnum> PreferebleSkills { get; set; } = request.PreferebleSkills;

    public CurrencyEnum Currency { get; set; } = request.Currency;

    public int Rate { get; set; } = request.Rate;

    public JobLocationEnum JobLocation { get; set; } = request.JobLocation;

    public Schedule Schedule { get; set; } = request.Schedule;
}
