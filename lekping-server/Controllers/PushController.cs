using lekping.server.Features.Push.Dtos;
using lekping.server.Features.Push.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace lekping.server.Controllers
{
    [ApiController]
    [Route("api/v1/push")]
    [Authorize]
    public sealed class PushController : ControllerBase
    {
        private readonly PushService _push;

        public PushController(PushService push) => _push = push;

        private Guid UserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest req, CancellationToken ct)
        {
            await _push.SaveSubscriptionAsync(UserId, req.Endpoint, req.P256DH, req.Auth, ct);
            return NoContent();
        }

        [HttpDelete("unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribeRequest req, CancellationToken ct)
        {
            await _push.RemoveSubscriptionAsync(UserId, req.Endpoint, ct);
            return NoContent();
        }

        [HttpDelete("unsubscribe-all")]
        public async Task<IActionResult> UnsubscribeAll(CancellationToken ct)
        {
            await _push.RemoveAllForUserAsync(UserId, ct);
            return NoContent();
        }

        [HttpGet("status")]
        public async Task<ActionResult<PushStatusDto>> Status(CancellationToken ct)
            => Ok(new PushStatusDto(await _push.AnyForUserAsync(UserId, ct)));

        [HttpPost("test")]
        public async Task<IActionResult> Test([FromBody] TestPushRequest? req, CancellationToken ct)
        {
            // if user has no subscriptions, return 404
            if (!await _push.AnyForUserAsync(UserId, ct))
                return NotFound(new { message = "No push subscriptions for current user." });

            var payload = new
            {
                title = req?.Title ?? "LekPing — test",
                body = req?.Body ?? "Works! 🎉 (only test notification)",
                url = req?.Url ?? $"{Request.Scheme}://{Request.Host}/meds"
            };

            await _push.SendToUserAsync(UserId, payload, ct);
            return Ok(new { sent = true });
        }
    }
}
