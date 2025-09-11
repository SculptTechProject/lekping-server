using lekping.server.Domain.Enums;

namespace lekping.server.Domain.Entities
{
    public class Schedule
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public Guid MedId { get; private set; }

        public decimal DoseValue { get; private set; }
        public string DoseUnit { get; private set; } = "tab";

        public string Timezone { get; private set; } = "Europe/Warsaw";
        public DateOnly StartDate { get; private set; }
        public DateOnly? EndDate { get; private set; }

        public Repeat Repeat { get; private set; } = Repeat.Daily;
        public List<string> Times { get; private set; } = new(); // "HH:mm"
        public int DaysOfWeekMask { get; private set; } // weekly

        public int RemindBeforeMinutes { get; private set; } = 10;
        public int WindowMinutes { get; private set; } = 45;

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        public User User { get; private set; } = null!;
        public Med Med { get; private set; } = null!;
        public ICollection<Intake> Intakes { get; private set; } = new List<Intake>();
    }
}
