using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventory");

            builder.HasKey(i => i.InventoryID);

            builder.Property(i => i.InventoryID)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.ProductID)
                .IsRequired();

            builder.Property(i => i.InitialStock)
                .IsRequired();

            builder.Property(i => i.MinStockLevel)
                .IsRequired();

            builder.Property(i => i.LastUpdated)
                .HasDefaultValueSql("GETUTCDATE()");

            // Configure common BaseEntity properties
            builder.Property(i => i.Status)
                .HasDefaultValue(true);

            builder.Property(i => i.CreatedBy)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("system");

            builder.Property(i => i.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(i => i.IsDeleted)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Product if Inventory exists

            // Seed Data
            builder.HasData(
                new Inventory
                {
                    InventoryID = 1,
                    ProductID = 1, // Assuming Product with ID 1 exists
                    InitialStock = 100,
                    CurrentStock = 100,
                    MinStockLevel = 10,
                    LastUpdated = DateTime.UtcNow,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Inventory
                {
                    InventoryID = 2,
                    ProductID = 2, // Assuming Product with ID 2 exists
                    InitialStock = 50,
                    MinStockLevel = 5,
                    LastUpdated = DateTime.UtcNow,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}
