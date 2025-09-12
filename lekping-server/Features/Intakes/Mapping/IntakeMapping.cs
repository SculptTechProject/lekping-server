using lekping.server.Domain.Entities;
using lekping.server.Features.Intakes.Dtos;

namespace lekping.server.Features.Intakes.Mapping
{
    public static class IntakeMapping
    {
        public static IntakeDto ToDto(this Intake e) => new(
            Id: e.Id,
            ScheduleId: e.ScheduleId,
            UserId: e.UserId,
            MedId: e.Schedule.MedId,
            MedBrandName: e.Schedule.Med.BrandName,
            DueAtUtc: e.DueAtUtc,
            RemindAtUtc: e.RemindAtUtc,
            Status: e.Status,
            SentAtUtc: e.SentAtUtc,
            TakenAtUtc: e.TakenAtUtc,
            SnoozedUntilUtc: e.SnoozedUntilUtc,
            CreatedAtUtc: e.CreatedAtUtc,
            DoseValue: e.Schedule.DoseValue,
            DoseUnit: e.Schedule.DoseUnit
        );
    }
}
