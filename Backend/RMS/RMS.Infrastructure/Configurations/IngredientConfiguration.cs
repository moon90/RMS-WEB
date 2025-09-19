using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients");

            builder.HasKey(i => i.IngredientID);

            builder.Property(i => i.IngredientID)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.QuantityAvailable)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(i => i.UnitID)
                .IsRequired();

            builder.Property(i => i.ReorderLevel)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(i => i.ReorderQuantity)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(i => i.Remarks)
                .HasMaxLength(250);

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
            builder.HasOne(i => i.Unit)
                .WithMany()
                .HasForeignKey(i => i.UnitID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Supplier)
                .WithMany()
                .HasForeignKey(i => i.SupplierID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            builder.HasData(
                new Ingredient
                {
                    IngredientID = 1,
                    Name = "Flour",
                    QuantityAvailable = 50.00m,
                    UnitID = 1, // Assuming Unit with ID 1 exists (e.g., "kg")
                    ReorderLevel = 10.00m,
                    ReorderQuantity = 20.00m,
                    SupplierID = 1, // Assuming Supplier with ID 1 exists
                    ExpireDate = DateTime.UtcNow.AddMonths(12),
                    Remarks = "All-purpose flour",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Ingredient
                {
                    IngredientID = 2,
                    Name = "Sugar",
                    QuantityAvailable = 25.00m,
                    UnitID = 1, // Assuming Unit with ID 1 exists (e.g., "kg")
                    ReorderLevel = 5.00m,
                    ReorderQuantity = 10.00m,
                    SupplierID = 1, // Assuming Supplier with ID 1 exists
                    ExpireDate = DateTime.UtcNow.AddMonths(24),
                    Remarks = "Granulated sugar",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}
