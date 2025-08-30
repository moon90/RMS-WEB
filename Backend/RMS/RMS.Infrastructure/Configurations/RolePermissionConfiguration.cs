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
                // Admin Role (ID 1) gets all permissions (1-29)
                new RolePermission { Id = 1, RoleID = 1, PermissionID = 1, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 2, RoleID = 1, PermissionID = 2, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 3, RoleID = 1, PermissionID = 3, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 4, RoleID = 1, PermissionID = 4, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 5, RoleID = 1, PermissionID = 5, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 6, RoleID = 1, PermissionID = 6, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 7, RoleID = 1, PermissionID = 7, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 8, RoleID = 1, PermissionID = 8, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 9, RoleID = 1, PermissionID = 9, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 10, RoleID = 1, PermissionID = 10, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 11, RoleID = 1, PermissionID = 11, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 12, RoleID = 1, PermissionID = 12, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 13, RoleID = 1, PermissionID = 13, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 14, RoleID = 1, PermissionID = 14, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 15, RoleID = 1, PermissionID = 15, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 16, RoleID = 1, PermissionID = 16, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 17, RoleID = 1, PermissionID = 17, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 18, RoleID = 1, PermissionID = 18, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 19, RoleID = 1, PermissionID = 19, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 20, RoleID = 1, PermissionID = 20, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 21, RoleID = 1, PermissionID = 21, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 22, RoleID = 1, PermissionID = 22, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 23, RoleID = 1, PermissionID = 23, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 24, RoleID = 1, PermissionID = 24, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 25, RoleID = 1, PermissionID = 25, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 26, RoleID = 1, PermissionID = 26, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 27, RoleID = 1, PermissionID = 27, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 28, RoleID = 1, PermissionID = 28, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 29, RoleID = 1, PermissionID = 29, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RolePermission { Id = 30, RoleID = 1, PermissionID = 30, AssignedBy = "System", AssignedAt = DateTime.UtcNow },

                // Manager Role (ID 2) permissions
                new RolePermission { Id = 31, RoleID = 2, PermissionID = 1, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // USER_VIEW
                new RolePermission { Id = 32, RoleID = 2, PermissionID = 3, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // USER_UPDATE
                new RolePermission { Id = 33, RoleID = 2, PermissionID = 11, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // MENU_VIEW
                new RolePermission { Id = 34, RoleID = 2, PermissionID = 13, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // MENU_UPDATE
                new RolePermission { Id = 35, RoleID = 2, PermissionID = 22, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // ROLE_VIEW
                new RolePermission { Id = 36, RoleID = 2, PermissionID = 18, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PERMISSION_VIEW
                new RolePermission { Id = 37, RoleID = 2, PermissionID = 17, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // AUDIT_LOG_VIEW
                new RolePermission { Id = 38, RoleID = 2, PermissionID = 28, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // INVENTORY_VIEW
                new RolePermission { Id = 39, RoleID = 2, PermissionID = 29, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // KITCHEN_VIEW
                new RolePermission { Id = 40, RoleID = 2, PermissionID = 30, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // ROLE_VIEW_MENUS

                // User Role (ID 3) permissions
                new RolePermission { Id = 41, RoleID = 3, PermissionID = 1, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // USER_VIEW
                new RolePermission { Id = 42, RoleID = 3, PermissionID = 11, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // MENU_VIEW
                new RolePermission { Id = 43, RoleID = 3, PermissionID = 28, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // INVENTORY_VIEW
                new RolePermission { Id = 44, RoleID = 3, PermissionID = 29, AssignedBy = "System", AssignedAt = DateTime.UtcNow },

                // Admin Role (ID 1) gets Category permissions
                new RolePermission { Id = 45, RoleID = 1, PermissionID = 31, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // CATEGORY_VIEW
                new RolePermission { Id = 46, RoleID = 1, PermissionID = 32, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // CATEGORY_CREATE
                new RolePermission { Id = 47, RoleID = 1, PermissionID = 33, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // CATEGORY_UPDATE
                new RolePermission { Id = 48, RoleID = 1, PermissionID = 34, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // CATEGORY_DELETE
                new RolePermission { Id = 49, RoleID = 1, PermissionID = 35, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // UNIT_VIEW
                new RolePermission { Id = 50, RoleID = 1, PermissionID = 36, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // UNIT_CREATE
                new RolePermission { Id = 51, RoleID = 1, PermissionID = 37, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // UNIT_UPDATE
                new RolePermission { Id = 52, RoleID = 1, PermissionID = 38, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // UNIT_DELETE

                // Admin Role (ID 1) gets Supplier permissions
                new RolePermission { Id = 53, RoleID = 1, PermissionID = 39, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // SUPPLIER_VIEW
                new RolePermission { Id = 54, RoleID = 1, PermissionID = 40, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // SUPPLIER_CREATE
                new RolePermission { Id = 55, RoleID = 1, PermissionID = 41, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // SUPPLIER_UPDATE
                new RolePermission { Id = 56, RoleID = 1, PermissionID = 42, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // SUPPLIER_DELETE

                // Admin Role (ID 1) gets Manufacturer permissions
                new RolePermission { Id = 57, RoleID = 1, PermissionID = 43, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // MANUFACTURER_VIEW
                new RolePermission { Id = 58, RoleID = 1, PermissionID = 44, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // MANUFACTURER_CREATE
                new RolePermission { Id = 59, RoleID = 1, PermissionID = 45, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // MANUFACTURER_UPDATE
                new RolePermission { Id = 60, RoleID = 1, PermissionID = 46, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // MANUFACTURER_DELETE

                // Admin Role (ID 1) gets Product permissions
                new RolePermission { Id = 61, RoleID = 1, PermissionID = 47, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PRODUCT_VIEW
                new RolePermission { Id = 62, RoleID = 1, PermissionID = 48, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PRODUCT_CREATE
                new RolePermission { Id = 63, RoleID = 1, PermissionID = 49, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PRODUCT_UPDATE
                new RolePermission { Id = 64, RoleID = 1, PermissionID = 50, AssignedBy = "System", AssignedAt = DateTime.UtcNow }  // PRODUCT_DELETE
            );
        }
    }
}
