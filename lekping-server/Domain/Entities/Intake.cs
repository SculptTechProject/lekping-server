using lekping.server.Domain.Enums;

namespace lekping.server.Domain.Entities
{
    public class Intake
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid ScheduleId { get; private set; }
        public Guid UserId { get; private set; }

        public DateTime DueAtUtc { get; private set; }
        public DateTime RemindAtUtc { get; private set; }

        public IntakeStatus Status { get; private set; } = IntakeStatus.Planned;
        public DateTime? SentAtUtc { get; private set; }
        public DateTime? TakenAtUtc { get; private set; }
        public DateTime? SnoozedUntilUtc { get; private set; }

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        public Schedule Schedule { get; private set; } = null!;
    }
}
