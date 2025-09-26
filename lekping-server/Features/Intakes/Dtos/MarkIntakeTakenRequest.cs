namespace lekping.server.Features.Intakes.Dtos
{
    public sealed record MarkIntakeTakenRequest(
        // let, even if user offline
        DateTime? TakenAtUtc
    );
}
