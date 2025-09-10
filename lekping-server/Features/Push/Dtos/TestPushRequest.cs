namespace lekping.server.Features.Push.Dtos
{
    public sealed record TestPushRequest(
        string? Title,
        string? Body,
        string? Url
    );
}
