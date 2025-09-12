using lekping.server.Domain.Enums;

namespace lekping.server.Features.Intakes.Dtos
{
    public sealed record IntakeDto(
        Guid Id,
        Guid ScheduleId,
        Guid UserId,
        Guid MedId,
        string MedBrandName,
        DateTime DueAtUtc,
        DateTime RemindAtUtc,
        IntakeStatus Status,
        DateTime? SentAtUtc,
        DateTime? TakenAtUtc,
        DateTime? SnoozedUntilUtc,
        DateTime CreatedAtUtc,
        decimal DoseValue,
        string DoseUnit
    );
}
