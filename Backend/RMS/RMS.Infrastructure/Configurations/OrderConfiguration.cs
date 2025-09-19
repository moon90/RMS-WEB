using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.OrderID);

            builder.Property(o => o.OrderDate)
                .HasColumnType("DATE"); // Map to DATE type in SQL

            builder.Property(o => o.OrderTime)
                .HasColumnType("VARCHAR(15)"); // Map to VARCHAR(15)

            builder.Property(o => o.TableName)
                .HasColumnType("VARCHAR(50)");

            builder.Property(o => o.WaiterName)
                .HasColumnType("VARCHAR(50)");

            builder.Property(o => o.OrderStatus)
                .HasColumnType("VARCHAR(20)");

            builder.Property(o => o.OrderType)
                .HasColumnType("VARCHAR(20)");

            builder.Property(o => o.Total)
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(o => o.DiscountAmount)
                .HasColumnType("DECIMAL(10,2)");

            builder.Property(o => o.DiscountPercentage)
                .HasColumnType("DECIMAL(10,2)");

            builder.Property(o => o.Received)
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(o => o.ChangeAmount)
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(o => o.PaymentStatus)
                .HasColumnType("VARCHAR(50)");

            builder.Property(o => o.PaymentMethod)
                .HasColumnType("VARCHAR(50)");

            builder.Property(o => o.AmountPaid)
                .HasColumnType("DECIMAL(18,2)");

            // Relationships
            builder.HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderID);

            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerID);

            // Seed Data
            builder.HasData(
                new Order
                {
                    OrderID = 1,
                    OrderDate = DateTime.Parse("2025-09-06"),
                    OrderTime = "12:30 PM",
                    TableName = "Table 5",
                    WaiterName = "John Doe",
                    OrderStatus = "Completed",
                    OrderType = "DineIn",
                    Total = 75.50m,
                    DiscountAmount = 0m,
                    DiscountPercentage = 0m,
                    PromotionID = null,
                    Received = 75.50m,
                    ChangeAmount = 0m,
                    DriverID = null,
                    CustomerID = 1,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Order
                {
                    OrderID = 2,
                    OrderDate = DateTime.Parse("2025-09-06"),
                    OrderTime = "01:00 PM",
                    TableName = "Takeout",
                    WaiterName = "Jane Smith",
                    OrderStatus = "Pending",
                    OrderType = "TakeOut",
                    Total = 30.00m,
                    DiscountAmount = 0m,
                    DiscountPercentage = 0m,
                    PromotionID = null,
                    Received = 0m,
                    ChangeAmount = 0m,
                    DriverID = null,
                    CustomerID = 2,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}