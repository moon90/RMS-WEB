using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class StockTransferConfiguration : IEntityTypeConfiguration<StockTransfer>
    {
        public void Configure(EntityTypeBuilder<StockTransfer> builder)
        {
            builder.HasKey(e => e.StockTransferID);
            builder.Property(e => e.TransferNumber).HasMaxLength(50);
            builder.Property(e => e.Remarks).HasMaxLength(500);

            builder.HasOne(d => d.FromBranch)
                .WithMany()
                .HasForeignKey(d => d.FromBranchID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.ToBranch)
                .WithMany()
                .HasForeignKey(d => d.ToBranchID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class StockTransferDetailConfiguration : IEntityTypeConfiguration<StockTransferDetail>
    {
        public void Configure(EntityTypeBuilder<StockTransferDetail> builder)
        {
            builder.HasKey(e => e.StockTransferDetailID);

            builder.HasOne(d => d.StockTransfer)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.StockTransferID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
