using lekping.server.Domain.Enums;

namespace lekping.server.Features.Schedules.Dtos
{
    public sealed record ScheduleDto(
        Guid Id,
        Guid MedId,
        string MedBrandName,
        decimal DoseValue,
        string DoseUnit,
        string Timezone,
        DateOnly StartDate,
        DateOnly? EndDate,
        Repeat Repeat,
        // rozbijam w DTO na tablicę
        string[] Times,
        int DaysOfWeekMask,
        int RemindBeforeMinutes,
        int WindowMinutes,
        string? Note,
        DateTime CreatedAtUtc
    );
}
