using BabySitting.Api.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabySitting.Api.Domain.Entities;

[Table("Offers")]
public class Offer
{
    [Key]
    public int Id { get; set; }

    public Guid UsertId { get; set; }

    public int FamilyId { get; set; }

    public HashSet<SkillsEnum> PreferebleSkills { get; set; } = new HashSet<SkillsEnum>();

    public CurrencyEnum Currency { get; set; }

    public int Rate { get; set; } 

    public JobLocationEnum JobLocation { get; set; }

    public Schedule Schedule { get; set; } = new Schedule();

}
