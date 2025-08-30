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
                // Admin Role (ID 1) gets all menu permissions
                new RoleMenu { Id = 1, RoleID = 1, MenuID = 1, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 2, RoleID = 1, MenuID = 2, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 3, RoleID = 1, MenuID = 3, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 4, RoleID = 1, MenuID = 4, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 5, RoleID = 1, MenuID = 5, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 6, RoleID = 1, MenuID = 6, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 7, RoleID = 1, MenuID = 7, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 8, RoleID = 1, MenuID = 8, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 9, RoleID = 1, MenuID = 9, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 10, RoleID = 1, MenuID = 10, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 11, RoleID = 1, MenuID = 11, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 12, RoleID = 1, MenuID = 12, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 13, RoleID = 1, MenuID = 13, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 14, RoleID = 1, MenuID = 14, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 15, RoleID = 1, MenuID = 15, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 16, RoleID = 1, MenuID = 16, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 17, RoleID = 1, MenuID = 17, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 18, RoleID = 1, MenuID = 18, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },
                new RoleMenu { Id = 19, RoleID = 1, MenuID = 19, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },

                // Manager Role (ID 2) menu permissions
                new RoleMenu { Id = 20, RoleID = 2, MenuID = 1, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Dashboard
                new RoleMenu { Id = 21, RoleID = 2, MenuID = 2, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // User Management
                new RoleMenu { Id = 22, RoleID = 2, MenuID = 3, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // User List
                new RoleMenu { Id = 23, RoleID = 2, MenuID = 5, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Roles
                new RoleMenu { Id = 24, RoleID = 2, MenuID = 6, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Role List
                new RoleMenu { Id = 25, RoleID = 2, MenuID = 14, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Menu Management
                new RoleMenu { Id = 26, RoleID = 2, MenuID = 15, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Menu List
                new RoleMenu { Id = 27, RoleID = 2, MenuID = 17, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Audit Logs
                new RoleMenu { Id = 28, RoleID = 2, MenuID = 18, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Inventory
                new RoleMenu { Id = 29, RoleID = 2, MenuID = 19, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Kitchen

                // User Role (ID 3) menu permissions
                new RoleMenu { Id = 30, RoleID = 3, MenuID = 1, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Dashboard
                new RoleMenu { Id = 31, RoleID = 3, MenuID = 18, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Inventory
                new RoleMenu { Id = 32, RoleID = 3, MenuID = 19, CanView = true, CanAdd = false, CanEdit = false, CanDelete = false, AssignedBy = "System", AssignedAt = DateTime.UtcNow },

                // Admin Role (ID 1) gets Category menus
                new RoleMenu { Id = 33, RoleID = 1, MenuID = 20, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Category Management
                new RoleMenu { Id = 34, RoleID = 1, MenuID = 21, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Category List
                new RoleMenu { Id = 35, RoleID = 1, MenuID = 22, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // Category Add
                new RoleMenu { Id = 36, RoleID = 1, MenuID = 23, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Unit Management
                new RoleMenu { Id = 37, RoleID = 1, MenuID = 24, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Unit List
                new RoleMenu { Id = 38, RoleID = 1, MenuID = 25, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // Unit Add

                // Admin Role (ID 1) gets Supplier menus
                new RoleMenu { Id = 39, RoleID = 1, MenuID = 26, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Supplier Management
                new RoleMenu { Id = 40, RoleID = 1, MenuID = 27, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Supplier List
                new RoleMenu { Id = 41, RoleID = 1, MenuID = 28, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // Supplier Add

                // Admin Role (ID 1) gets Manufacturer menus
                new RoleMenu { Id = 45, RoleID = 1, MenuID = 29, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Manufacturer Management
                new RoleMenu { Id = 46, RoleID = 1, MenuID = 30, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Manufacturer List
                new RoleMenu { Id = 47, RoleID = 1, MenuID = 31, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // Manufacturer Add

                // Admin Role (ID 1) gets Product menus
                new RoleMenu { Id = 48, RoleID = 1, MenuID = 32, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Product Management
                new RoleMenu { Id = 49, RoleID = 1, MenuID = 33, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Product List
                new RoleMenu { Id = 50, RoleID = 1, MenuID = 34, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }  // Product Add
            );
        }
    }
}
