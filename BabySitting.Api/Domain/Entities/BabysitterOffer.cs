using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BabySitting.Api.Domain.Enums;

namespace BabySitting.Api.Domain.Entities;

[Table("BabysitterOffers")]
public class BabysitterOffer
{
    [Key]
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

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

    public BabysitterOffer(BabysitterAccountCompletion.Command request)
    {
        UserId = request.UserId;
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
    }

    public BabysitterOffer() { }
}
