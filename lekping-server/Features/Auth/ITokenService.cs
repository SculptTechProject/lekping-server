using lekping.server.Domain.Entities;

namespace lekping.server.Features.Auth
{
    public interface ITokenService
    {
        (string token, DateTime expiresAtUtc) CreateAccessToken(User user);
    }
}
