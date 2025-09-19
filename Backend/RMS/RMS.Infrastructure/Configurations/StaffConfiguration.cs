using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.ToTable("Staff");

            builder.HasKey(s => s.StaffID);

            builder.Property(s => s.StaffID)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.StaffName)
                .HasMaxLength(100);

            builder.Property(s => s.StaffPhone)
                .HasMaxLength(20);

            builder.Property(s => s.StaffRole)
                .HasMaxLength(100);

            // Configure common BaseEntity properties
            builder.Property(s => s.Status)
                .HasDefaultValue(true);

            builder.Property(s => s.CreatedBy)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("system");

            builder.Property(s => s.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(s => s.IsDeleted)
                .HasDefaultValue(false);

            // Seed Data
            builder.HasData(
                new Staff
                {
                    StaffID = 1,
                    StaffName = "Alice Johnson",
                    StaffPhone = "111-222-3333",
                    StaffRole = "Manager",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Staff
                {
                    StaffID = 2,
                    StaffName = "Bob Williams",
                    StaffPhone = "444-555-6666",
                    StaffRole = "Chef",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}
