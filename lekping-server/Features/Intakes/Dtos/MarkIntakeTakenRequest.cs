namespace lekping.server.Features.Intakes.Dtos
{
    public sealed record MarkIntakeTakenRequest(
        // pozwól przesłać czas, gdy UI było offline
        DateTime? TakenAtUtc
    );
}
