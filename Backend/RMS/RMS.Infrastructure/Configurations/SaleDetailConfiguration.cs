using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class SaleDetailConfiguration : IEntityTypeConfiguration<SaleDetail>
    {
        public void Configure(EntityTypeBuilder<SaleDetail> builder)
        {
            builder.HasKey(sd => sd.SaleDetailID);

            builder.Property(sd => sd.Quantity)
                .IsRequired();

            builder.Property(sd => sd.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(sd => sd.Discount)
                .HasColumnType("decimal(18,2)");

            builder.Property(sd => sd.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(sd => sd.Product)
                .WithMany()
                .HasForeignKey(sd => sd.ProductID);

            // Seed Data
            builder.HasData(
                new SaleDetail
                {
                    SaleDetailID = 1,
                    SaleID = 1, // Links to Sale with ID 1
                    ProductID = 1, // Assuming Product with ID 1 exists
                    Quantity = 2,
                    UnitPrice = 10.00m,
                    Discount = 0.00m,
                    TotalAmount = 20.00m,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new SaleDetail
                {
                    SaleDetailID = 2,
                    SaleID = 1, // Links to Sale with ID 1
                    ProductID = 2, // Assuming Product with ID 2 exists
                    Quantity = 1,
                    UnitPrice = 20.00m,
                    Discount = 0.00m,
                    TotalAmount = 20.00m,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}