using lekping.server.Domain.Enums;

namespace lekping.server.Domain.Entities
{
    public class Med
    {
        private Med() { }
        public Guid UserId { get; private set; }
        public User? User { get; private set; }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string BrandName { get; private set; } = null!;
        public string GenericName { get; private set; } = null!;
        public decimal StrengthValue { get; private set; }
        public string StrengthUnit { get; private set; } = null!;
        public DosageForm Form { get; private set; }
        public int PackageSize { get; private set; }
        public string? Ean { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Med(Guid userId, string brandName, string genericName,
               decimal strengthValue, string strengthUnit,
               DosageForm form, int packageSize, string? ean)
        {
            UserId = userId;
            BrandName = brandName;
            GenericName = genericName;
            StrengthValue = strengthValue;
            StrengthUnit = strengthUnit;
            Form = form;
            PackageSize = packageSize;
            Ean = ean;
        }

        public void Update(string brand, string generic, decimal strengthValue,
                           string strengthUnit, DosageForm form, int packageSize, string? ean)
        {
            BrandName = brand;
            GenericName = generic;
            StrengthValue = strengthValue;
            StrengthUnit = strengthUnit;
            Form = form;
            PackageSize = packageSize;
            Ean = ean;
        }

    }
}
