using BabySitting.Api.Domain.Enums;

namespace BabySitting.Api.Domain.Entities;

public class Job
{
    public int Id { get; set; }

    public Guid UsertId { get; set; }

    public HashSet<SkillsEnum> SkillsEnums { get; set; } = new HashSet<SkillsEnum>();

    public CurrencyEnum Currency { get; set; }

    public int Rate {  get; set; } 

    public JobLocationEnum JobLocation { get; set; }

    public Schedule Schedule { get; set; } = new Schedule();

}
