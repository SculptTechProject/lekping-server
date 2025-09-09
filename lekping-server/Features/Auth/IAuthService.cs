using lekping.server.Domain.Entities;

namespace LekPing.Server.Features.Auth;

public interface IAuthService
{
    Task<User> RegisterAsync(string email, string name, string password, CancellationToken ct);
    Task<User?> LoginAsync(string email, string password, CancellationToken ct);
}
