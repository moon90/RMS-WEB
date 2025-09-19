using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(s => s.SaleID);

            builder.Property(s => s.SaleDate)
                .IsRequired();

            builder.Property(s => s.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(s => s.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.FinalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(s => s.PaymentMethod)
                .HasMaxLength(50);

            builder.HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerID);

            builder.HasMany(s => s.SaleDetails)
                .WithOne(sd => sd.Sale)
                .HasForeignKey(sd => sd.SaleID);

            // Seed Data
            builder.HasData(
                new Sale
                {
                    SaleID = 1,
                    SaleDate = DateTime.UtcNow,
                    CustomerID = 1, // Assuming Customer with ID 1 exists
                    TotalAmount = 40.00m,
                    DiscountAmount = 0.00m,
                    FinalAmount = 40.00m,
                    PaymentMethod = "Cash",
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