using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class SplitPaymentConfiguration : IEntityTypeConfiguration<SplitPayment>
    {
        public void Configure(EntityTypeBuilder<SplitPayment> builder)
        {
            builder.ToTable("SplitPayments");

            builder.HasKey(sp => sp.SplitPaymentID);

            builder.Property(sp => sp.SplitPaymentID)
                .ValueGeneratedOnAdd();

            builder.Property(sp => sp.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(sp => sp.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(sp => sp.Sale)
                .WithMany()
                .HasForeignKey(sp => sp.SaleID);

            // Seed Data
            builder.HasData(
                new SplitPayment
                {
                    SplitPaymentID = 1,
                    SaleID = 1, // Assuming Sale with ID 1 exists
                    Amount = 20.00m,
                    PaymentMethod = "Cash",
                    CreatedOn = DateTime.UtcNow
                },
                new SplitPayment
                {
                    SplitPaymentID = 2,
                    SaleID = 1, // Assuming Sale with ID 1 exists
                    Amount = 20.00m,
                    PaymentMethod = "Card",
                    CreatedOn = DateTime.UtcNow
                }
            );
        }
    }
}
