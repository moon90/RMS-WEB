using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class PurchaseDetailConfiguration : IEntityTypeConfiguration<PurchaseDetail>
    {
        public void Configure(EntityTypeBuilder<PurchaseDetail> builder)
        {
            builder.HasKey(pd => pd.PurchaseDetailID);

            builder.Property(pd => pd.Quantity)
                .IsRequired();

            builder.Property(pd => pd.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(pd => pd.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(pd => pd.Product)
                .WithMany()
                .HasForeignKey(pd => pd.ProductID);

            // Seed Data
            builder.HasData(
                new PurchaseDetail
                {
                    PurchaseDetailID = 1,
                    PurchaseID = 1, // Links to Purchase with ID 1
                    ProductID = 1, // Assuming Product with ID 1 exists
                    Quantity = 5,
                    UnitPrice = 5.00m,
                    TotalAmount = 25.00m,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new PurchaseDetail
                {
                    PurchaseDetailID = 2,
                    PurchaseID = 1, // Links to Purchase with ID 1
                    ProductID = 2, // Assuming Product with ID 2 exists
                    Quantity = 3,
                    UnitPrice = 10.00m,
                    TotalAmount = 30.00m,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}