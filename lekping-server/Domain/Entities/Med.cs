using lekping.server.Domain.Enums;

namespace lekping.server.Domain.Entities
{
    public class Med
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string BrandName { get; private set; } = null!;
        public string GenericName { get; private set; } = null!;
        public decimal StrengthValue { get; private set; }
        public string StrengthUnit { get; private set; } = null!;
        public DosageForm Form { get; private set; }
        public int PackageSize { get; private set; }
        public string? Ean { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Med(string brandName, string genericName, decimal strengthValue, string strengthUnit, DosageForm form, int packageSize, string? ean)
        {
            BrandName = brandName.Trim();
            GenericName = genericName.Trim();
            StrengthValue = strengthValue;
            StrengthUnit = strengthUnit.Trim();
            Form = form;
            PackageSize = packageSize;
            Ean = string.IsNullOrWhiteSpace(ean) ? null : ean.Trim();
        }

        public void Update(string brand, string generic, decimal val, string unit, DosageForm form, int pack, string? ean)
            => (BrandName, GenericName, StrengthValue, StrengthUnit, Form, PackageSize, Ean)
               = (brand.Trim(), generic.Trim(), val, unit.Trim(), form, pack, string.IsNullOrWhiteSpace(ean) ? null : ean.Trim());
    }
}
