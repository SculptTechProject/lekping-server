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
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // push notification
            modelBuilder.Entity<PushSubscription>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Endpoint).IsRequired();
                e.Property(x => x.P256DH).IsRequired();
                e.Property(x => x.Auth).IsRequired();
                e.Property(x => x.CreatedAt).IsRequired();

                // Nie chcemy duplikatów dla tego samego usera i endpointu
                e.HasIndex(x => new { x.UserId, x.Endpoint }).IsUnique();

                // (opcjonalnie) FK do User
                e.HasOne<User>()
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // med
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
