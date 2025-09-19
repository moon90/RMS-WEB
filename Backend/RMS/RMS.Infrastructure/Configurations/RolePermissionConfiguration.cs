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
                new RolePermission { Id = 64, RoleID = 1, PermissionID = 50, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // PRODUCT_DELETE

                // Admin Role (ID 1) gets Customer permissions
                new RolePermission { Id = 65, RoleID = 1, PermissionID = 51, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // CUSTOMER_VIEW
                new RolePermission { Id = 66, RoleID = 1, PermissionID = 52, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // CUSTOMER_CREATE
                new RolePermission { Id = 67, RoleID = 1, PermissionID = 53, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // CUSTOMER_UPDATE
                new RolePermission { Id = 68, RoleID = 1, PermissionID = 54, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // CUSTOMER_DELETE

                // Admin Role (ID 1) gets Staff permissions
                new RolePermission { Id = 69, RoleID = 1, PermissionID = 55, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // STAFF_VIEW
                new RolePermission { Id = 70, RoleID = 1, PermissionID = 56, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // STAFF_CREATE
                new RolePermission { Id = 71, RoleID = 1, PermissionID = 57, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // STAFF_UPDATE
                new RolePermission { Id = 72, RoleID = 1, PermissionID = 58, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // STAFF_DELETE

                // Admin Role (ID 1) gets Inventory permissions
                new RolePermission { Id = 73, RoleID = 1, PermissionID = 59, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // INVENTORY_VIEW
                new RolePermission { Id = 74, RoleID = 1, PermissionID = 60, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // INVENTORY_CREATE
                new RolePermission { Id = 75, RoleID = 1, PermissionID = 61, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // INVENTORY_UPDATE
                new RolePermission { Id = 76, RoleID = 1, PermissionID = 62, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // INVENTORY_DELETE
                new RolePermission { Id = 89, RoleID = 1, PermissionID = 75, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // INVENTORY_LOW_STOCK_VIEW

                // Admin Role (ID 1) gets Stock Transaction permissions
                new RolePermission { Id = 77, RoleID = 1, PermissionID = 63, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // STOCK_TRANSACTION_VIEW
                new RolePermission { Id = 78, RoleID = 1, PermissionID = 64, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // STOCK_TRANSACTION_CREATE
                new RolePermission { Id = 79, RoleID = 1, PermissionID = 65, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // STOCK_TRANSACTION_UPDATE
                new RolePermission { Id = 80, RoleID = 1, PermissionID = 66, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // STOCK_TRANSACTION_DELETE

                // Admin Role (ID 1) gets Ingredient permissions
                new RolePermission { Id = 81, RoleID = 1, PermissionID = 67, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // INGREDIENT_VIEW
                new RolePermission { Id = 82, RoleID = 1, PermissionID = 68, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // INGREDIENT_CREATE
                new RolePermission { Id = 83, RoleID = 1, PermissionID = 69, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // INGREDIENT_UPDATE
                new RolePermission { Id = 84, RoleID = 1, PermissionID = 70, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // INGREDIENT_DELETE

                // Admin Role (ID 1) gets Product Ingredient permissions
                new RolePermission { Id = 85, RoleID = 1, PermissionID = 71, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PRODUCT_INGREDIENT_VIEW
                new RolePermission { Id = 86, RoleID = 1, PermissionID = 72, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PRODUCT_INGREDIENT_CREATE
                new RolePermission { Id = 87, RoleID = 1, PermissionID = 73, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PRODUCT_INGREDIENT_UPDATE
                new RolePermission { Id = 88, RoleID = 1, PermissionID = 74, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // PRODUCT_INGREDIENT_DELETE

                // Admin Role (ID 1) gets Order permissions
                new RolePermission { Id = 90, RoleID = 1, PermissionID = 76, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // ORDER_VIEW
                new RolePermission { Id = 91, RoleID = 1, PermissionID = 77, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // ORDER_CREATE
                new RolePermission { Id = 92, RoleID = 1, PermissionID = 78, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // ORDER_UPDATE
                new RolePermission { Id = 93, RoleID = 1, PermissionID = 79, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // ORDER_DELETE

                // Admin Role (ID 1) gets Dining Table permissions
                new RolePermission { Id = 94, RoleID = 1, PermissionID = 80, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // DINING_TABLE_VIEW
                new RolePermission { Id = 95, RoleID = 1, PermissionID = 81, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // DINING_TABLE_CREATE
                new RolePermission { Id = 96, RoleID = 1, PermissionID = 82, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // DINING_TABLE_UPDATE
                new RolePermission { Id = 97, RoleID = 1, PermissionID = 83, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // DINING_TABLE_DELETE

                // Admin Role (ID 1) gets Promotion permissions
                new RolePermission { Id = 98, RoleID = 1, PermissionID = 87, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PROMOTION_VIEW
                new RolePermission { Id = 99, RoleID = 1, PermissionID = 88, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PROMOTION_CREATE
                new RolePermission { Id = 100, RoleID = 1, PermissionID = 89, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PROMOTION_UPDATE
                new RolePermission { Id = 101, RoleID = 1, PermissionID = 90, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // PROMOTION_DELETE

                // Admin Role (ID 1) gets Purchase permissions
                new RolePermission { Id = 102, RoleID = 1, PermissionID = 91, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PURCHASE_VIEW
                new RolePermission { Id = 103, RoleID = 1, PermissionID = 92, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PURCHASE_CREATE
                new RolePermission { Id = 104, RoleID = 1, PermissionID = 93, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // PURCHASE_UPDATE
                new RolePermission { Id = 105, RoleID = 1, PermissionID = 94, AssignedBy = "System", AssignedAt = DateTime.UtcNow },  // PURCHASE_DELETE

                // Admin Role (ID 1) gets Sales permissions
                new RolePermission { Id = 106, RoleID = 1, PermissionID = 95, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // SALE_VIEW
                new RolePermission { Id = 107, RoleID = 1, PermissionID = 96, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // SALE_CREATE
                new RolePermission { Id = 108, RoleID = 1, PermissionID = 97, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // SALE_UPDATE
                new RolePermission { Id = 109, RoleID = 1, PermissionID = 98, AssignedBy = "System", AssignedAt = DateTime.UtcNow }  // SALE_DELETE
            );
        }
    }
}
