using System.ComponentModel.DataAnnotations;
using lekping.server.Domain.Enums;

namespace lekping.server.Features.Meds.Dtos
{
    public sealed record CreateMedRequest(
        [Required, StringLength(200)] string BrandName,
        [Required, StringLength(200)] string GenericName,
        [Range(0.001, 1000000)] decimal StrengthValue,
        [Required, StringLength(50)] string StrengthUnit,
        DosageForm Form,
        [Range(1, 1000000)] int PackageSize,
        string? Ean
        );
}
