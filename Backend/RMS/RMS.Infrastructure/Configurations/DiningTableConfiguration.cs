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
                new DiningTable
                {
                    TableID = 1,
                    TableName = "Table 1",
                    Status = true, // Keep existing Status
                    DiningTableStatus = DiningTableStatusEnum.Available,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new DiningTable
                {
                    TableID = 2,
                    TableName = "Table 2",
                    Status = true, // Keep existing Status
                    DiningTableStatus = DiningTableStatusEnum.Available,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new DiningTable
                {
                    TableID = 3,
                    TableName = "Table 3",
                    Status = true, // Keep existing Status
                    DiningTableStatus = DiningTableStatusEnum.Available,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new DiningTable
                {
                    TableID = 4,
                    TableName = "Table 4",
                    Status = true, // Keep existing Status
                    DiningTableStatus = DiningTableStatusEnum.Available,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new DiningTable
                {
                    TableID = 5,
                    TableName = "Table 5",
                    Status = true, // Keep existing Status
                    DiningTableStatus = DiningTableStatusEnum.Available,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}