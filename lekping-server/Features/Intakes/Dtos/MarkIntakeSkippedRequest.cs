using System.ComponentModel.DataAnnotations;

namespace lekping.server.Features.Intakes.Dtos
{
    public sealed record MarkIntakeSkippedRequest(
        [StringLength(200)] string? Reason
    );
}
