using System.ComponentModel.DataAnnotations;

namespace lekping.server.Features.Intakes.Dtos
{
    public sealed record SnoozeIntakeRequest(
        [Range(1, 240)] int Minutes
    );
}
