using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Configurations
{
    public class RoleMenuConfiguration : IEntityTypeConfiguration<RoleMenu>
    {
        public void Configure(EntityTypeBuilder<RoleMenu> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Role)
                   .WithMany(r => r.RoleMenus)
                   .HasForeignKey(e => e.RoleID);

            builder.HasOne(e => e.Menu)
                   .WithMany(m => m.RoleMenus)
                   .HasForeignKey(e => e.MenuID);

            builder.Property(rm => rm.AssignedBy)
            .IsRequired(false);

            builder.HasData(
                new RoleMenu
                {
                    Id = 1,
                    RoleID = 1, // Admin role
                    MenuID = 1, // Dashboard menu
                    CanView = true,
                    CanAdd = true,
                    CanEdit = true,
                    CanDelete = true,
                    AssignedBy = "System", // ✅ Required field
                    AssignedAt = DateTime.UtcNow
                },
                new RoleMenu
                {
                    Id = 2,
                    RoleID = 1, // Admin role
                    MenuID = 2, // Users menu
                    CanView = true,
                    CanAdd = true,
                    CanEdit = true,
                    CanDelete = true,
                    AssignedBy = "System", // ✅ Required field
                    AssignedAt = DateTime.UtcNow
                }
            );
        }
    }
}
