using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class ProductIngredientConfiguration : IEntityTypeConfiguration<ProductIngredient>
    {
        public void Configure(EntityTypeBuilder<ProductIngredient> builder)
        {
            builder.ToTable("ProductIngredients");

            builder.HasKey(pi => pi.ProductIngredientID);

            builder.Property(pi => pi.ProductIngredientID)
                .ValueGeneratedOnAdd();

            builder.Property(pi => pi.ProductID)
                .IsRequired();

            builder.Property(pi => pi.IngredientID)
                .IsRequired();

            builder.Property(pi => pi.Quantity)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(pi => pi.UnitID)
                .IsRequired();

            builder.Property(pi => pi.Remarks)
                .HasMaxLength(250);

            // Configure common BaseEntity properties
            builder.Property(pi => pi.Status)
                .HasDefaultValue(true);

            builder.Property(pi => pi.CreatedBy)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("system");

            builder.Property(pi => pi.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(pi => pi.IsDeleted)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(pi => pi.Product)
                .WithMany()
                .HasForeignKey(pi => pi.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pi => pi.Ingredient)
                .WithMany()
                .HasForeignKey(pi => pi.IngredientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pi => pi.Unit)
                .WithMany()
                .HasForeignKey(pi => pi.UnitID)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            builder.HasData(
                new ProductIngredient
                {
                    ProductIngredientID = 1,
                    ProductID = 1, // Assuming Product with ID 1 exists
                    IngredientID = 1, // Assuming Ingredient with ID 1 exists (Flour)
                    Quantity = 0.5m,
                    UnitID = 1, // Assuming Unit with ID 1 exists (kg)
                    Remarks = "Used for bread",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ProductIngredient
                {
                    ProductIngredientID = 2,
                    ProductID = 1, // Assuming Product with ID 1 exists
                    IngredientID = 2, // Assuming Ingredient with ID 2 exists (Sugar)
                    Quantity = 0.1m,
                    UnitID = 1, // Assuming Unit with ID 1 exists (kg)
                    Remarks = "Used for sweetness",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}
