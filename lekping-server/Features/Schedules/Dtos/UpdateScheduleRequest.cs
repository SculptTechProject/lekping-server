using lekping.server.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace lekping.server.Features.Schedules.Dtos
{
    public sealed record UpdateScheduleRequest(
        [Required] Guid Id,
        [Range(0.001, 100000)] decimal DoseValue,
        [Required, StringLength(32)] string DoseUnit,
        [StringLength(64)] string? Timezone,
        [Required] DateOnly StartDate,
        DateOnly? EndDate,
        [Required] Repeat Repeat,
        [MinLength(1), MaxLength(8)]
        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d(,([01]\d|2[0-3]):[0-5]\d){0,7}$",
            ErrorMessage = "Enter times as 'HH:mm' and separate them with commas.")]
        string TimesCsv,
        [Range(0, 127)] int DaysOfWeekMask = 0,
        [Range(0, 240)] int RemindBeforeMinutes = 10,
        [Range(0, 240)] int WindowMinutes = 45,
        [StringLength(256)] string? Notes = null
    );
}
