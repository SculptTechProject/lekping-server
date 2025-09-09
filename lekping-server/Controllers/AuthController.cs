using lekping.server.Features.Auth;
using LekPing.Server.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static lekping.server.Features.Auth.Dtos.AuthDtos;

namespace lekping.server.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly ITokenService _tokens;

        public AuthController(IAuthService auth, ITokenService token)
        {
            _auth = auth;
            _tokens = token;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest req, CancellationToken ct)
        {
            var user = await _auth.RegisterAsync(req.Email, req.Name, req.Password, ct);
            var (token, exp) = _tokens.CreateAccessToken(user);
            return Created("", new AuthResponse(token, exp));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest req, CancellationToken ct)
        {
            var user = await _auth.LoginAsync(req.Email, req.Password, ct);
            if (user is null) return Unauthorized(new { error = "Invalid credentials" });

            var (token, exp) = _tokens.CreateAccessToken(user);
            return Ok(new AuthResponse(token, exp));
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue("sub");
            var email = User.FindFirstValue(ClaimTypes.Email)
                        ?? User.Identity?.Name;
            var role = User.FindFirstValue(ClaimTypes.Role)
                        ?? User.FindFirstValue("role");

            return Ok(new { id, email, role });
        }
    }
}
