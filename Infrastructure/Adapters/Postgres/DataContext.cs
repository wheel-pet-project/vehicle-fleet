using Domain.ModelAggregate;
using Domain.VehicleAggregate;
using Infrastructure.Adapters.Postgres.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Adapters.Postgres;

public sealed class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<OutboxEvent> Outbox { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ModelEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StatusEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxEventTypeConfiguration());
    }
}

internal sealed class ModelEntityTypeConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.ToTable("model");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever().HasColumnName("id").IsRequired();

        builder.OwnsOne(x => x.Brand, cfg => { cfg.Property(x => x.Name).HasColumnName("brand").IsRequired(); });

        builder.OwnsOne(x => x.CarModel, cfg => { cfg.Property(x => x.Name).HasColumnName("car_model").IsRequired(); });

        builder.OwnsOne(x => x.Category,
            cfg => { cfg.Property(x => x.Character).HasColumnName("category").IsRequired(); });

        builder.OwnsOne(x => x.Tariff, cfg =>
        {
            cfg.Property(x => x.PricePerMinute).HasColumnName("price_per_minute").IsRequired();
            cfg.Property(x => x.PricePerHour).HasColumnName("price_per_hour").IsRequired();
            cfg.Property(x => x.PricePerDay).HasColumnName("price_per_day").IsRequired();
        });

        builder.Ignore(x => x.DomainEvents);
    }
}

internal sealed class VehicleEntityTypeConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicle");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever().HasColumnName("id").IsRequired();
        builder.Property(x => x.ModelId).HasColumnName("model_id").IsRequired();

        builder.HasOne<Model>().WithMany().HasForeignKey(x => x.ModelId).HasConstraintName("FK_model_id").IsRequired();

        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey("status_id")
            .HasConstraintName("FK_status_id")
            .IsRequired();

        builder.OwnsOne(x => x.PlateNumber,
            cfg => { cfg.Property(x => x.Value).HasColumnName("plate_number").IsRequired(); });

        builder.OwnsOne(x => x.Color, cfg => { cfg.Property(x => x.Name).HasColumnName("color").IsRequired(); });

        builder.OwnsOne(x => x.Vin, cfg => { cfg.Property(x => x.Number).HasColumnName("vin").IsRequired(); });

        builder.OwnsOne(x => x.FuelLevel,
            cfg => { cfg.Property(x => x.Percents).HasColumnName("fuel_level_percents").IsRequired(); });

        builder.OwnsOne(x => x.Location, cfg =>
        {
            cfg.Property(x => x.Latitude).HasColumnName("location_latitude").IsRequired();
            cfg.Property(x => x.Longitude).HasColumnName("location_longitude").IsRequired();
        });

        builder.Ignore(x => x.DomainEvents);
    }
}

internal sealed class StatusEntityTypeConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.ToTable("vehicle_status");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever().HasColumnName("id").IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();

        builder.HasData(Status.All());
    }
}

internal sealed class OutboxEventTypeConfiguration : IEntityTypeConfiguration<OutboxEvent>
{
    public void Configure(EntityTypeBuilder<OutboxEvent> builder)
    {
        builder.ToTable("outbox");

        builder.HasKey(x => x.EventId);

        builder.Property(x => x.EventId).HasColumnName("event_id").ValueGeneratedNever().IsRequired();
        builder.Property(x => x.Type).HasColumnName("type").IsRequired();
        builder.Property(x => x.Content).HasColumnName("content").IsRequired();
        builder.Property(x => x.OccurredOnUtc).HasColumnName("occurred_on_utc").IsRequired();
        builder.Property(x => x.ProcessedOnUtc).HasColumnName("processed_on_utc").IsRequired(false);
    }
}