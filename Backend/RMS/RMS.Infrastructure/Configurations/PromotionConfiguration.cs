using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotions");

            builder.HasKey(p => p.PromotionID);

            builder.Property(p => p.CouponCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.DiscountAmount)
                .HasColumnType("DECIMAL(10,2)")
                .HasDefaultValue(0);

            builder.Property(p => p.DiscountPercentage)
                .HasColumnType("DECIMAL(10,2)")
                .HasDefaultValue(0);

            builder.Property(p => p.Description)
                .HasMaxLength(255);

            builder.Property(p => p.ValidFrom)
                .IsRequired();

            builder.Property(p => p.ValidTo)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);

            // Audit fields
            builder.Property(p => p.Status)
                .HasDefaultValue(true);

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(100)
                .IsRequired()
                .HasDefaultValue("system");

            builder.Property(p => p.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            // Seed Data
            builder.HasData(
                new Promotion
                {
                    PromotionID = 1,
                    CouponCode = "SUMMER20",
                    DiscountAmount = 0,
                    DiscountPercentage = 20.00m,
                    Description = "20% off on all items for summer",
                    ValidFrom = DateTime.UtcNow,
                    ValidTo = DateTime.UtcNow.AddMonths(1),
                    IsActive = true,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Promotion
                {
                    PromotionID = 2,
                    CouponCode = "HALFOFF",
                    DiscountAmount = 5.00m,
                    DiscountPercentage = 0,
                    Description = "$5 off on orders over $20",
                    ValidFrom = DateTime.UtcNow,
                    ValidTo = DateTime.UtcNow.AddDays(7),
                    IsActive = true,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}