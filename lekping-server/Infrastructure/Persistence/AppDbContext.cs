using lekping.server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace lekping.server.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Med> Meds => Set<Med>();
        public DbSet<PushSubscription> PushSubscriptions => Set<PushSubscription>();


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // user
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(e => e.Id);
                e.HasIndex(e => e.Email).IsUnique();
                e.Property(e => e.Email).IsRequired().HasMaxLength(256);
                e.Property(e => e.Name).IsRequired();
                e.Property(e => e.PasswordHash).IsRequired();
                e.Property(e => e.Roles).HasColumnType("text[]");
                e.Property(x => x.Timezone).IsRequired().HasMaxLength(64);
                e.Property(x => x.CreatedAt)
                 .HasColumnType("timestamptz")
                 .IsRequired();
            });

            // push notification
            modelBuilder.Entity<PushSubscription>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Endpoint).IsRequired();
                e.Property(x => x.P256DH).IsRequired();
                e.Property(x => x.Auth).IsRequired();
                e.Property(x => x.CreatedAt)
                 .HasColumnType("timestamptz")
                 .IsRequired();

                // Nie chcemy duplikatów dla tego samego usera i endpointu
                e.HasIndex(x => new { x.UserId, x.Endpoint }).IsUnique();

                // K do User
                e.HasOne<User>()
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // med
            modelBuilder.Entity<Med>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasOne(x => x.User)
                    .WithMany(u => u.Meds)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.BrandName).IsRequired().HasMaxLength(256);
                e.Property(x => x.GenericName).IsRequired().HasMaxLength(256);
                e.Property(x => x.StrengthValue).HasPrecision(10, 2).IsRequired();
                e.Property(x => x.StrengthUnit).IsRequired().HasMaxLength(32);
                e.Property(x => x.Form).HasConversion<int>().IsRequired();
                e.Property(x => x.PackageSize).IsRequired();
                e.Property(x => x.Ean).HasMaxLength(32);
                e.Property(x => x.CreatedAt)
                 .HasColumnType("timestamptz")
                 .IsRequired();

                // unikalność „tego samego leku” u JEDNEGO usera
                e.HasIndex(x => new { x.UserId, x.BrandName, x.StrengthValue, x.StrengthUnit, x.Form, x.PackageSize })
                 .IsUnique();

                // EAN bywa unikatowy – ale „per user”:
                e.HasIndex(x => new { x.UserId, x.Ean }).IsUnique();
            });

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
