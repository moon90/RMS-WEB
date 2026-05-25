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
                    StaffName = "Chef Marco Pierre",
                    StaffPhone = "555-1001",
                    StaffRole = "Executive Chef",
                    HourlyRate = 45.00m,
                    BranchID = 1,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Staff
                {
                    StaffID = 2,
                    StaffName = "Elena Rodriguez",
                    StaffPhone = "555-1002",
                    StaffRole = "Floor Manager",
                    HourlyRate = 35.00m,
                    BranchID = 1,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Staff
                {
                    StaffID = 3,
                    StaffName = "James Wilson",
                    StaffPhone = "555-2001",
                    StaffRole = "Head Waiter",
                    HourlyRate = 22.00m,
                    BranchID = 2,
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}
