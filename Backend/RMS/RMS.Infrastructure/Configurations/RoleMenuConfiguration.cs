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
                new RoleMenu { Id = 57, RoleID = 1, MenuID = 43, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Alerts

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
                new RoleMenu { Id = 50, RoleID = 1, MenuID = 34, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // Product Add

                // Admin Role (ID 1) gets Customer menus
                new RoleMenu { Id = 51, RoleID = 1, MenuID = 35, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Customer Management
                new RoleMenu { Id = 52, RoleID = 1, MenuID = 36, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Customer List
                new RoleMenu { Id = 53, RoleID = 1, MenuID = 37, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // Customer Add

                // Admin Role (ID 1) gets Staff menus
                new RoleMenu { Id = 54, RoleID = 1, MenuID = 38, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Staff Management
                new RoleMenu { Id = 55, RoleID = 1, MenuID = 39, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Staff List
                new RoleMenu { Id = 56, RoleID = 1, MenuID = 40, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // Staff Add

                // Admin Role (ID 1) gets Order Management menus
                new RoleMenu { Id = 69, RoleID = 1, MenuID = 53, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Order Management
                new RoleMenu { Id = 70, RoleID = 1, MenuID = 54, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Order List
                new RoleMenu { Id = 71, RoleID = 1, MenuID = 55, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // Order Add

                // Admin Role (ID 1) gets Dining Table Management menus
                new RoleMenu { Id = 72, RoleID = 1, MenuID = 56, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Dining Table Management
                new RoleMenu { Id = 73, RoleID = 1, MenuID = 57, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Dining Table List
                new RoleMenu { Id = 74, RoleID = 1, MenuID = 58, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // Dining Table Add

                // Admin Role (ID 1) gets Promotions menus
                new RoleMenu { Id = 75, RoleID = 1, MenuID = 59, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Promotions Management
                new RoleMenu { Id = 76, RoleID = 1, MenuID = 60, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Promotion List
                new RoleMenu { Id = 77, RoleID = 1, MenuID = 61, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // Promotion Add

                // Admin Role (ID 1) gets Purchase menus
                new RoleMenu { Id = 78, RoleID = 1, MenuID = 62, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Purchase Management
                new RoleMenu { Id = 79, RoleID = 1, MenuID = 63, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Purchase List
                new RoleMenu { Id = 80, RoleID = 1, MenuID = 64, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Purchase Add
                new RoleMenu { Id = 81, RoleID = 1, MenuID = 65, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Purchase Edit
                new RoleMenu { Id = 82, RoleID = 1, MenuID = 66, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Purchase Detail

                // Admin Role (ID 1) gets Sales menus
                new RoleMenu { Id = 83, RoleID = 1, MenuID = 67, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Sales Management
                new RoleMenu { Id = 84, RoleID = 1, MenuID = 68, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Sales List
                new RoleMenu { Id = 85, RoleID = 1, MenuID = 69, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Sales Add
                new RoleMenu { Id = 86, RoleID = 1, MenuID = 70, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Sales Edit
                new RoleMenu { Id = 87, RoleID = 1, MenuID = 71, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, AssignedBy = "System", AssignedAt = DateTime.UtcNow }  // Sale Detail
            );
        }
    }
}
