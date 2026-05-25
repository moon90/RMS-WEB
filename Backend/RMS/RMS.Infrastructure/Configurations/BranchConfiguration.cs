using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(e => e.BranchID);
            builder.Property(e => e.BranchName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.BranchCode).HasMaxLength(50);
            builder.Property(e => e.Address).HasMaxLength(500);
            builder.Property(e => e.Phone).HasMaxLength(20);
            builder.Property(e => e.Email).HasMaxLength(100);

            // Seed Branches
            builder.HasData(
                new Branch { 
                    BranchID = 1, 
                    BranchName = "Main Headquarters", 
                    BranchCode = "MAIN-HQ", 
                    Address = "123 Business Central, New York", 
                    Phone = "+1-800-RMS-MAIN", 
                    Email = "hq@rms-global.com",
                    IsMainBranch = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow
                },
                new Branch { 
                    BranchID = 2, 
                    BranchName = "Downtown Express", 
                    BranchCode = "DT-EXP", 
                    Address = "456 Urban Square, Manhattan", 
                    Phone = "+1-800-RMS-DOWNTOWN", 
                    Email = "downtown@rms-global.com",
                    IsMainBranch = false,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow
                }
            );
        }
    }
}
