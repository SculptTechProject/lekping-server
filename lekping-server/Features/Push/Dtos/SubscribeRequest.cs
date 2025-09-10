namespace lekping.server.Features.Push.Dtos
{
    public sealed record SubscribeRequest(string Endpoint, string P256DH, string Auth);
}
