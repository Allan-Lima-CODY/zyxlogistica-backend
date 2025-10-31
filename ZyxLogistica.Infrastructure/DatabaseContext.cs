using Microsoft.EntityFrameworkCore;
using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Infrastructure;

public class DatabaseContext : DbContext
{
    public DbSet<Driver> Drivers { get; set; } = null!;
    public DbSet<Truck> Trucks { get; set; } = null!;
    public DbSet<Inventory> Inventories { get; set; } = null!;
    public DbSet<InboundEntry> InboundEntries { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderInventory> OrderInventories { get; set; } = null!;
    public DbSet<Expedition> Expeditions { get; set; } = null!;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Driver
        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(200);
            entity.Property(d => d.Phone).IsRequired().HasMaxLength(20);
            entity.Property(d => d.Cnh).IsRequired().HasMaxLength(20).ValueGeneratedNever();
            entity.Property(d => d.CnhCategory).HasConversion<int>().IsRequired().ValueGeneratedNever();
            entity.Property(d => d.Active).IsRequired();
            entity.Property(d => d.CreatedAt).IsRequired();
            entity.Property(d => d.UpdatedAt).IsRequired(false);
        });

        // Truck
        modelBuilder.Entity<Truck>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.LicensePlate).IsRequired().HasMaxLength(20);
            entity.Property(t => t.Model).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Year).IsRequired();
            entity.Property(t => t.CapacityKg).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(t => t.Available).IsRequired();
            entity.Property(t => t.CreatedAt).IsRequired();
            entity.Property(t => t.UpdatedAt).IsRequired(false);
        });

        // Inventory
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ProductCode).IsRequired().HasMaxLength(50);
            entity.Property(i => i.Description).IsRequired().HasMaxLength(200);
            entity.Property(i => i.Quantity).IsRequired();
            entity.Property(i => i.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(i => i.Active).IsRequired();
            entity.Property(i => i.CreatedAt).IsRequired();
            entity.Property(i => i.UpdatedAt).IsRequired(false);
        });

        // InboundEntry
        modelBuilder.Entity<InboundEntry>(entity =>
        {
            entity.HasKey(ie => ie.Id);
            entity.Property(ie => ie.Reference).IsRequired().HasMaxLength(100);
            entity.Property(ie => ie.SupplierName).IsRequired().HasMaxLength(200);
            entity.Property(ie => ie.Observation).HasMaxLength(500);
            entity.Property(ie => ie.CreatedAt).IsRequired();
            entity.Property(ie => ie.UpdatedAt).IsRequired(false);

            entity.HasOne(ie => ie.Inventory)
                  .WithMany()
                  .HasForeignKey("InventoryId")
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
            entity.Property(o => o.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(o => o.Status).IsRequired();
            entity.Property(o => o.CreatedAt).IsRequired();
            entity.Property(o => o.UpdatedAt).IsRequired(false);

            entity.HasMany(o => o.Items)
                  .WithOne(oi => oi.Order)
                  .HasForeignKey("OrderId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // OrderInventory
        modelBuilder.Entity<OrderInventory>(entity =>
        {
            entity.HasKey(oi => oi.Id);
            entity.Property(oi => oi.Quantity).IsRequired();

            entity.HasOne(oi => oi.Inventory)
                  .WithMany()
                  .HasForeignKey("InventoryId")
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Expedition
        modelBuilder.Entity<Expedition>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DeliveryForecast).IsRequired();
            entity.Property(e => e.Observation).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired(false);

            entity.HasOne(e => e.Order)
                  .WithMany()
                  .HasForeignKey("OrderId")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Driver)
                  .WithMany()
                  .HasForeignKey("DriverId")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Truck)
                  .WithMany()
                  .HasForeignKey("TruckId")
                  .OnDelete(DeleteBehavior.Restrict);
        });

    }
}
