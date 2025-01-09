using System.ComponentModel.DataAnnotations.Schema;

namespace BabySitting.Api.Domain.Entities;

[Table("Schedules")]
public class Schedule
{
    public int Id { get; set; }
     
    public Guid UserId { get; set; }

    public bool MondayMorning { get; set; } = false;
    public bool MondayAfternoon { get; set; } = false;
    public bool MondayEvening { get; set; } = false;
    public bool MondayNight { get; set; } = false;

    public bool TuesdayMorning { get; set; } = false;
    public bool TuesdayAfternoon { get; set; } = false;
    public bool TuesdayEvening { get; set; } = false;
    public bool TuesdayNight { get; set; } = false;

    public bool WednesdayMorning { get; set; } = false;
    public bool WednesdayAfternoon { get; set; } = false;
    public bool WednesdayEvening { get; set; } = false;
    public bool WednesdayNight { get; set; } = false;

    public bool ThursdayMorning { get; set; } = false;
    public bool ThursdayAfternoon { get; set; } = false;
    public bool ThursdayEvening { get; set; } = false;
    public bool ThursdayNight { get; set; } = false;

    public bool FridayMorning { get; set; } = false;
    public bool FridayAfternoon { get; set; } = false;
    public bool FridayEvening { get; set; } = false;
    public bool FridayNight { get; set; } = false;

    public bool SaturdayMorning { get; set; } = false;
    public bool SaturdayAfternoon { get; set; } = false;
    public bool SaturdayEvening { get; set; } = false;
    public bool SaturdayNight { get; set; } = false;

    public bool SundayMorning { get; set; } = false;
    public bool SundayAfternoon { get; set; } = false;
    public bool SundayEvening { get; set; } = false;
    public bool SundayNight { get; set; } = false;

    public Schedule() { }
}
