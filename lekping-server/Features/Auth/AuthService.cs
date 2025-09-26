using lekping.server.Domain.Entities;
using lekping.server.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LekPing.Server.Features.Auth;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<User> _hasher;

    public AuthService(AppDbContext db, IPasswordHasher<User> hasher)
    { _db = db; _hasher = hasher; }

    public async Task<User> RegisterAsync(string email, string Name, string password, CancellationToken ct)
    {
        var e = email.ToLowerInvariant();
        if (await _db.Users.AnyAsync(u => u.Email == e, ct))
            throw new InvalidOperationException("Email already registered.");

        var user = new User(e);
        user.SetName(Name);
        user.SetPasswordHash(_hasher.HashPassword(user, password));
        // optional: user.SetRole("User");
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return user;
    }

    public async Task<User?> LoginAsync(string email, string password, CancellationToken ct)
    {
        var e = email.ToLowerInvariant();
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == e, ct);
        if (user is null) return null;

        var ok = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return ok == PasswordVerificationResult.Success ? user : null;
    }
}
