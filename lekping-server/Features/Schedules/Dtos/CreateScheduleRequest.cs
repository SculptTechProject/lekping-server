using lekping.server.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace lekping.server.Features.Schedules.Dtos
{
    public sealed record CreateScheduleRequest(
        [Required] Guid MedId,
        [Range(0.001, 100000)] decimal DoseValue,
        [Required, StringLength(32)] string DoseUnit,
        // jeżeli null, weź z profilu usera
        [StringLength(64)] string? Timezone,
        // daty lokalne wg Timezone
        [Required] DateOnly StartDate,
        DateOnly? EndDate,
        [Required] Repeat Repeat,
        // "HH:mm" lokalnie; min 1, max 8 wpisów
        [MinLength(1), MaxLength(8)]
        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d(,([01]\d|2[0-3]):[0-5]\d){0,7}$",
            ErrorMessage = "Czasy podaj jako 'HH:mm' i oddziel przecinkami.")]
        string TimesCsv,
        // dla Weekly – bitmask: Mon=1, Tue=2, ... Sun=64
        [Range(0, 127)] int DaysOfWeekMask = 0,
        [Range(0, 240)] int RemindBeforeMinutes = 10,
        [Range(0, 240)] int WindowMinutes = 45,
        [StringLength(256)] string? Notes = null
    );
}
