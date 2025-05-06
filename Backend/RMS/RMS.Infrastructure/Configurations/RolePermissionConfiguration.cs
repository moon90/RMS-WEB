using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Role)
                   .WithMany(r => r.RolePermissions)
                   .HasForeignKey(e => e.RoleID);

            builder.HasOne(e => e.Permission)
                   .WithMany(p => p.RolePermissions)
                   .HasForeignKey(e => e.PermissionID);

            builder.Property(rm => rm.AssignedBy)
                    .IsRequired(false);

            builder.HasData(
                new RolePermission
                {
                    Id = -1,
                    RoleID = 1,
                    PermissionID = 1,
                    AssignedBy = "System", // ✅ Required field
                    AssignedAt = DateTime.UtcNow
                },
                new RolePermission
                {
                    Id = -2,
                    RoleID = 1,
                    PermissionID = 2,
                    AssignedBy = "System", // ✅ Required field
                    AssignedAt = DateTime.UtcNow
                }
            );
        }
    }
}
