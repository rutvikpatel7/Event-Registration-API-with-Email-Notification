using Microsoft.EntityFrameworkCore;
using EventRegistrationAPI.Models;

namespace EventRegistrationAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Event
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Capacity).IsRequired();
            });

            // Configure Registration
            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(200);
                entity.Property(r => r.Email).IsRequired().HasMaxLength(256);
                entity.Property(r => r.RegisteredAt).IsRequired();

                entity.HasOne(r => r.Event)
                    .WithMany(e => e.Registrations)
                    .HasForeignKey(r => r.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(r => new { r.EventId, r.Email }).IsUnique();
            });
        }
    }
}