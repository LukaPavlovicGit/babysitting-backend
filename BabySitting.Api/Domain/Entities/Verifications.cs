using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabySitting.Api.Domain.Entities;

[Table("Verifications")]
public class Verifications
{
    [Key]
    public int Id { get; set; }

    public Guid UserId { get; set; } 

    public bool GovernmentIdProvided { get; set; } = false;

    public bool EmailVerified { get; set; } = false;
    
    public bool PhonePhoneVerified { get; set; } = false;

    public bool GoogleAccountVerified { get; set; } = false;

    public bool FacebookAccountVerified {  get; set; } = false;
    
    public bool LinkedInAccountVerified { get; set; } = false;


    public DateTime GovernmentIdProvidedAt { get; set; }

    public DateTime EmailVerifiedAt { get; set; }

    public DateTime PhonePhoneVerifiedAt { get; set; }

    public DateTime GoogleAccountVerifiedAt { get; set; }

    public DateTime FacebookAccountVerifiedAt { get; set; }

    public DateTime LinkedInAccountVerifiedAt { get; set; }

    public Verifications() { }

}
