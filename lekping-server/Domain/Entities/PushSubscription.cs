namespace lekping.server.Domain.Entities
{
    public sealed class PushSubscription
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }

        public string Endpoint { get; set; } = default!;
        public string P256DH { get; set; } = default!;
        public string Auth { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
