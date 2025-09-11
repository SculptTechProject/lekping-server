using lekping.server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lekping.server.Features.Meds.Persistence
{
    public class MedConfiguration : IEntityTypeConfiguration<Med>
    {
        public void Configure(EntityTypeBuilder<Med> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.BrandName).IsRequired().HasMaxLength(200);
            b.Property(x => x.GenericName).IsRequired().HasMaxLength(200);
            b.Property(x => x.StrengthUnit).IsRequired().HasMaxLength(50);
            b.Property(x => x.StrengthValue).IsRequired().HasPrecision(18, 6);
            b.Property(x => x.Form).IsRequired();
            b.Property(x => x.PackageSize).IsRequired();
            b.Property(x => x.CreatedAt).IsRequired();

            // relacja z User
            b.HasOne(m => m.User)
             .WithMany(u => u.Meds)
             .HasForeignKey(m => m.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            // indeksy
            b.HasIndex(m => new { m.UserId, m.Ean }).IsUnique();
            b.HasIndex(m => new { m.UserId, m.BrandName, m.StrengthValue, m.StrengthUnit, m.Form, m.PackageSize }).IsUnique();
        }
    }
}
