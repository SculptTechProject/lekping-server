using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebPush;

using ClientPushSubscription = WebPush.PushSubscription;                 // do wysyłki (WebPush)
using DbPushSubscription = lekping.server.Domain.Entities.PushSubscription; // do EF/DB
using lekping.server.Infrastructure.Persistence;

namespace lekping.server.Features.Push.Service
{
    public sealed class PushService
    {
        private readonly AppDbContext _db;
        private readonly VapidOptions _opts;
        private readonly WebPushClient _client = new();

        public PushService(AppDbContext db, IOptions<VapidOptions> opts)
        {
            _db = db;
            _opts = opts.Value;
        }

        public async Task SaveSubscriptionAsync(
            Guid userId,
            string endpoint,
            string p256dh,
            string auth,
            CancellationToken ct)
        {
            var exists = await _db.PushSubscriptions
                .AnyAsync(x => x.UserId == userId && x.Endpoint == endpoint, ct);

            if (!exists)
            {
                _db.PushSubscriptions.Add(new DbPushSubscription
                {
                    UserId = userId,
                    Endpoint = endpoint,
                    P256DH = p256dh,
                    Auth = auth
                });

                await _db.SaveChangesAsync(ct);
            }
        }

        public async Task SendToUserAsync(Guid userId, object payload, CancellationToken ct)
        {
            var subs = await _db.PushSubscriptions
                                .Where(x => x.UserId == userId)
                                .ToListAsync(ct);

            var vapid = new VapidDetails(_opts.Subject, _opts.PublicKey, _opts.PrivateKey);
            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            foreach (var s in subs)
            {
                var sub = new ClientPushSubscription(s.Endpoint, s.P256DH, s.Auth);

                try
                {
                    await _client.SendNotificationAsync(sub, json, vapid, ct);
                }
                catch (WebPushException ex) when (
                    ex.StatusCode == System.Net.HttpStatusCode.Gone ||
                    ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // martwy endpoint – czyścimy
                    _db.PushSubscriptions.Remove(s);
                }
            }

            await _db.SaveChangesAsync(ct);
        }

        public async Task RemoveSubscriptionAsync(Guid userId, string endpoint, CancellationToken ct)
        {
            var sub = await _db.PushSubscriptions
                .SingleOrDefaultAsync(x => x.UserId == userId && x.Endpoint == endpoint, ct);
            if (sub is null) return;

            _db.PushSubscriptions.Remove(sub);
            await _db.SaveChangesAsync(ct);
        }

        public async Task RemoveAllForUserAsync(Guid userId, CancellationToken ct)
        {
            var q = _db.PushSubscriptions.Where(x => x.UserId == userId);
            _db.PushSubscriptions.RemoveRange(q);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<bool> AnyForUserAsync(Guid userId, CancellationToken ct)
            => await _db.PushSubscriptions.AnyAsync(x => x.UserId == userId, ct);

        public sealed class VapidOptions
        {
            public string Subject { get; set; } = default!;
            public string PublicKey { get; set; } = default!;
            public string PrivateKey { get; set; } = default!;
        }
    }
}
