using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(p => p.PurchaseID);

            builder.Property(p => p.PurchaseDate)
                .IsRequired();

            builder.Property(p => p.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
                .HasMaxLength(50);

            builder.HasOne(p => p.Supplier)
                .WithMany()
                .HasForeignKey(p => p.SupplierID);

            builder.HasMany(p => p.PurchaseDetails)
                .WithOne(pd => pd.Purchase)
                .HasForeignKey(pd => pd.PurchaseID);

            // Seed Data
            builder.HasData(
                new Purchase
                {
                    PurchaseID = 1,
                    PurchaseDate = DateTime.UtcNow,
                    SupplierID = 1, // Assuming Supplier with ID 1 exists
                    TotalAmount = 55.00m, // 25.00 + 30.00
                    PaymentMethod = "Credit Card",
                    CategoryId = 1,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}