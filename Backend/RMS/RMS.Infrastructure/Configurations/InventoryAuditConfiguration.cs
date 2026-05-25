using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class InventoryAuditConfiguration : IEntityTypeConfiguration<InventoryAudit>
    {
        public void Configure(EntityTypeBuilder<InventoryAudit> builder)
        {
            builder.HasKey(e => e.InventoryAuditID);
            builder.Property(e => e.AuditorName).HasMaxLength(100);
            builder.Property(e => e.Remarks).HasMaxLength(500);

            builder.HasMany(e => e.Details)
                .WithOne(d => d.Audit)
                .HasForeignKey(d => d.InventoryAuditID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class InventoryAuditDetailConfiguration : IEntityTypeConfiguration<InventoryAuditDetail>
    {
        public void Configure(EntityTypeBuilder<InventoryAuditDetail> builder)
        {
            builder.HasKey(e => e.InventoryAuditDetailID);
            
            builder.HasOne(d => d.Ingredient)
                .WithMany()
                .HasForeignKey(d => d.IngredientID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
