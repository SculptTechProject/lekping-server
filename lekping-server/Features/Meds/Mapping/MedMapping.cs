using lekping.server.Domain.Entities;
using lekping.server.Features.Meds.Dtos;

namespace lekping.server.Features.Meds.Mapping
{
    public static class MedMapping
    {
        public static MedDto ToDto(this Med e) =>
          new(
            e.Id,
            e.BrandName,
            e.GenericName,
            $"{e.StrengthValue:G} {e.StrengthUnit}",
            e.Form,
            e.PackageSize,
            e.Ean
        );

    }
}
