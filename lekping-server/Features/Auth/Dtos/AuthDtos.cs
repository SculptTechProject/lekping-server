using System.ComponentModel.DataAnnotations;

namespace lekping.server.Features.Auth.Dtos
{
    public class AuthDtos
    {
        public sealed record RegisterRequest(
            [Required, EmailAddress] string Email,
            [Required, MinLength(3), MaxLength(50)] string Name,
            [Required, MinLength(6), MaxLength(100)] string Password
        );

        public sealed record LoginRequest(
            [Required, EmailAddress] string Email,
            [Required, MinLength(6), MaxLength(100)] string Password
        );

        public sealed record AuthResponse(
            string AccessToken,
            DateTime ExpiresAtUtc
        );
    }
}
