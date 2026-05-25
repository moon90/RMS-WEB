using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll> builder)
        {
            builder.HasKey(e => e.PayrollID);
            
            builder.HasOne(d => d.Staff)
                .WithMany()
                .HasForeignKey(d => d.StaffID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
