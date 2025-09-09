using lekping.server.Domain.Entities;
using lekping.server.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace lekping.server.Features.Auth
{
    public sealed class TokenService : ITokenService
    {
        private readonly JwtOptions _opt;
        private readonly SigningCredentials _creds;

        public TokenService(IOptions<JwtOptions> opt)
        {
            _opt = opt.Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key));
            _creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public (string token, DateTime expiresAtUtc) CreateAccessToken(User user)
        {
            var now = DateTime.UtcNow;
            var nbf = now.AddSeconds(-5);
            var exp = now.AddMinutes(Math.Max(1, _opt.ExpireMinutes));

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var jwt = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: _opt.Audience,
                claims: claims,
                notBefore: nbf,
                expires: exp,
                signingCredentials: _creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(jwt), exp);
        }
    }
}
