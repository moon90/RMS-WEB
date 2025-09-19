using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od => od.OrderDetailID);

            builder.Property(od => od.Quantity)
                .IsRequired();

            builder.Property(od => od.Price)
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(od => od.DiscountPrice)
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(od => od.Amount)
                .HasColumnType("DECIMAL(18,2)");

            // Relationships
            builder.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID);

            builder.HasOne(od => od.Product)
                .WithMany() // Assuming Product does not have a collection of OrderDetails
                .HasForeignKey(od => od.ProductID);

            // Seed Data
            builder.HasData(
                new OrderDetail
                {
                    OrderDetailID = 1,
                    OrderID = 1,
                    ProductID = 1, // Assuming Product with ID 1 exists
                    Quantity = 2,
                    Price = 25.00m,
                    DiscountPrice = 0m,
                    Amount = 50.00m,
                    PromotionDetailID = null,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new OrderDetail
                {
                    OrderDetailID = 2,
                    OrderID = 1,
                    ProductID = 2, // Assuming Product with ID 2 exists
                    Quantity = 1,
                    Price = 25.50m,
                    DiscountPrice = 0m,
                    Amount = 25.50m,
                    PromotionDetailID = null,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new OrderDetail
                {
                    OrderDetailID = 3,
                    OrderID = 2,
                    ProductID = 3, // Assuming Product with ID 3 exists
                    Quantity = 1,
                    Price = 30.00m,
                    DiscountPrice = 0m,
                    Amount = 30.00m,
                    PromotionDetailID = null,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}