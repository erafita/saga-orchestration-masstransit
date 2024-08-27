using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Sagas;

namespace Newsletter.Api.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<NewsletterOnboardingSagaData> SagaData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewsletterOnboardingSagaData>()
            .HasKey(n => n.CorrelationId);

        modelBuilder.Entity<Subscriber>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Subscriber>()
            .HasIndex(s => s.Email)
            .IsUnique();

        modelBuilder.Entity<Subscriber>()
            .Property(s => s.Email)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
        modelBuilder.AddInboxStateEntity();
    }
}