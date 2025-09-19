using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLog");

            builder.HasKey(al => al.Id);

            builder.Property(al => al.Id)
                .ValueGeneratedOnAdd();

            builder.Property(al => al.Action)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(al => al.EntityType) // Corrected from EntityName
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(al => al.EntityId) // Corrected from EntityID
                .IsRequired()
                .HasMaxLength(100); // Added MaxLength

            builder.Property(al => al.PerformedBy) // Corrected from ChangedBy
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(al => al.PerformedAt) // Corrected from ChangedOn
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(al => al.Details) // Added Details property
                .HasColumnType("nvarchar(max)");

            // No seed data for AuditLog as it's system-generated
        }
    }
}