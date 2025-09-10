using lekping.server.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace lekping.server.Features.Meds.Dtos
{
    public sealed record UpdateMedRequest(
        [Required] Guid Id,
        [Required, StringLength(200)] string BrandName,
        [Required, StringLength(200)] string GenericName,
        [Range(0.001, 100000)] decimal StrengthValue,
        [Required, StringLength(50)] string StrengthUnit,
        DosageForm Form,
        [Range(1, 100000)] int PackageSize,
        string? Ean
        );
}
