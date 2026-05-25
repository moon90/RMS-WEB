using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;
using RMS.Domain.Enum;

namespace RMS.Infrastructure.Configurations
{
    public class DiningTableConfiguration : IEntityTypeConfiguration<DiningTable>
    {
        public void Configure(EntityTypeBuilder<DiningTable> builder)
        {
            builder.HasKey(dt => dt.TableID);

            builder.Property(dt => dt.TableName)
                .HasColumnType("VARCHAR(100)");

            builder.Property(dt => dt.DiningTableStatus)
                .HasConversion<string>(); // Map enum to string in DB

            // Seed Data
            builder.HasData(
                new DiningTable { TableID = 1, TableName = "Window Booth A1", BranchID = 1, Status = true, DiningTableStatus = DiningTableStatusEnum.Available, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new DiningTable { TableID = 2, TableName = "Garden Terrace T1", BranchID = 1, Status = true, DiningTableStatus = DiningTableStatusEnum.Available, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new DiningTable { TableID = 3, TableName = "VIP Lounge V1", BranchID = 1, Status = true, DiningTableStatus = DiningTableStatusEnum.Available, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new DiningTable { TableID = 4, TableName = "Express Table 01", BranchID = 2, Status = true, DiningTableStatus = DiningTableStatusEnum.Available, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new DiningTable { TableID = 5, TableName = "Express Table 02", BranchID = 2, Status = true, DiningTableStatus = DiningTableStatusEnum.Available, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new DiningTable { TableID = 6, TableName = "Global Support Node", BranchID = null, Status = true, DiningTableStatus = DiningTableStatusEnum.Available, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
            );
        }
    }
}