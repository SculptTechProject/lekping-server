using lekping.server.Domain.Enums;

namespace lekping.server.Features.Meds.Dtos
{
    public sealed record MedDto(
        Guid Id,
        string BrandName,
        string GenericName,
        string Strength,
        DosageForm Form,
        int PackageSize,
        string? Ean
    );
}
