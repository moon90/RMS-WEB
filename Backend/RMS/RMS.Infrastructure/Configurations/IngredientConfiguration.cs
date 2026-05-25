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
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.CostPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            builder.Property(i => i.UnitID)
                .IsRequired();

            builder.Property(i => i.ReorderLevel)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.ReorderQuantity)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

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
                    Name = "Japanese A5 Wagyu Beef",
                    QuantityAvailable = 25.50m,
                    CostPrice = 120.00m,
                    UnitID = 1, // kg
                    ReorderLevel = 5.00m,
                    ReorderQuantity = 10.00m,
                    SupplierID = 1,
                    ExpireDate = DateTime.UtcNow.AddDays(7),
                    BranchID = 1,
                    Remarks = "Flash frozen, premium grade",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Ingredient
                {
                    IngredientID = 2,
                    Name = "Black Truffle Oil",
                    QuantityAvailable = 5.00m,
                    CostPrice = 45.00m,
                    UnitID = 2, // Liters
                    ReorderLevel = 1.00m,
                    ReorderQuantity = 2.00m,
                    SupplierID = 2,
                    ExpireDate = DateTime.UtcNow.AddMonths(6),
                    BranchID = 1,
                    Remarks = "Infused extra virgin oil",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Ingredient
                {
                    IngredientID = 3,
                    Name = "Express Blend Coffee Beans",
                    QuantityAvailable = 15.00m,
                    CostPrice = 18.00m,
                    UnitID = 1, // kg
                    ReorderLevel = 3.00m,
                    ReorderQuantity = 5.00m,
                    SupplierID = 1,
                    ExpireDate = DateTime.UtcNow.AddMonths(3),
                    BranchID = 2,
                    Remarks = "Custom dark roast for Downtown node",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}
