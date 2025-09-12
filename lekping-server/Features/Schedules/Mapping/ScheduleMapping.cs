using lekping.server.Domain.Entities;
using lekping.server.Features.Schedules.Dtos;

namespace lekping.server.Features.Schedules.Mapping
{
    public static class ScheduleMapping
    {
        public static ScheduleDto ToDto(this Schedule e) => new(
            e.Id,
            e.MedId,
            e.Med?.BrandName ?? "",
            e.DoseValue,
            e.DoseUnit,
            e.Timezone,
            e.StartDate,
            e.EndDate,
            e.Repeat,
            e.Times.ToArray(),
            e.DaysOfWeekMask,
            e.RemindBeforeMinutes,
            e.WindowMinutes,
            e.Note,
            e.CreatedAtUtc
        );
    }
}
