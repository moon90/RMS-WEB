using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace RMS.Infrastructure.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.MenuName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.MenuPath).IsRequired().HasMaxLength(200);
            builder.Property(e => e.MenuIcon).HasMaxLength(100);
            builder.Property(e => e.ControllerName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.ActionName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.ModuleName).IsRequired().HasMaxLength(100);

            builder.HasOne(m => m.ParentMenu)
                   .WithMany(m => m.ChildMenus)
                   .HasForeignKey(m => m.ParentID)
                   .OnDelete(DeleteBehavior.Restrict);

            // Seed initial menus
            builder.HasData(
                    new Menu { Id = 1, MenuName = "Dashboard", MenuPath = "/dashboard", MenuIcon = "FaHome", ControllerName = "Dashboard", ActionName = "Index", ModuleName = "Dashboard", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 2, MenuName = "User Management", MenuPath = "/users", MenuIcon = "FaUsers", ControllerName = "Users", ActionName = "Index", ModuleName = "UserManagement", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 3, MenuName = "User List", ParentID = 2, MenuPath = "/users/list", MenuIcon = "FaListUl", ControllerName = "Users", ActionName = "List", ModuleName = "UserManagement", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 4, MenuName = "User Add", ParentID = 2, MenuPath = "/users/add", MenuIcon = "FaPlus", ControllerName = "Users", ActionName = "Add", ModuleName = "UserManagement", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 5, MenuName = "Roles", ParentID = 2, MenuPath = "/roles", MenuIcon = "FaUserShield", ControllerName = "Roles", ActionName = "Index", ModuleName = "UserManagement", DisplayOrder = 3, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 6, MenuName = "Role List", ParentID = 5, MenuPath = "/roles/list", MenuIcon = "FaListUl", ControllerName = "Roles", ActionName = "List", ModuleName = "UserManagement", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 7, MenuName = "Role Add", ParentID = 5, MenuPath = "/roles/add", MenuIcon = "FaPlus", ControllerName = "Roles", ActionName = "Add", ModuleName = "UserManagement", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 8, MenuName = "User Access Role", ParentID = 5, MenuPath = "/roles/access_role", MenuIcon = "FaUserShield", ControllerName = "Roles", ActionName = "AccessRole", ModuleName = "UserManagement", DisplayOrder = 3, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 9, MenuName = "Role Permissions", ParentID = 5, MenuPath = "/roles/role_permissions", MenuIcon = "FaCog", ControllerName = "Permissions", ActionName = "Setup", ModuleName = "UserManagement", DisplayOrder = 4, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 10, MenuName = "Menu Assignments", ParentID = 5, MenuPath = "/roles/menu_assignments", MenuIcon = "FaClipboardList", ControllerName = "Menus", ActionName = "Setup", ModuleName = "UserManagement", DisplayOrder = 5, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 11, MenuName = "Permissions", ParentID = 2, MenuPath = "/permissions", MenuIcon = "FaCog", ControllerName = "Permissions", ActionName = "Index", ModuleName = "UserManagement", DisplayOrder = 4, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 12, MenuName = "Permission List", ParentID = 11, MenuPath = "/permissions/list", MenuIcon = "FaListUl", ControllerName = "Permissions", ActionName = "List", ModuleName = "UserManagement", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 13, MenuName = "Permission Add", ParentID = 11, MenuPath = "/permissions/add", MenuIcon = "FaPlus", ControllerName = "Permissions", ActionName = "Add", ModuleName = "UserManagement", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 14, MenuName = "Menu Management", MenuPath = "/menus", MenuIcon = "FaClipboardList", ControllerName = "Menus", ActionName = "Index", ModuleName = "MenuManagement", DisplayOrder = 3, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 15, MenuName = "Menu List", ParentID = 14, MenuPath = "/menus/list", MenuIcon = "FaListUl", ControllerName = "Menus", ActionName = "List", ModuleName = "MenuManagement", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 16, MenuName = "Menu Add", ParentID = 14, MenuPath = "/menus/add", MenuIcon = "FaPlus", ControllerName = "Menus", ActionName = "Add", ModuleName = "MenuManagement", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 17, MenuName = "Audit Logs", MenuPath = "/audit-logs", MenuIcon = "FaFileAlt", ControllerName = "AuditLogs", ActionName = "Index", ModuleName = "System", DisplayOrder = 4, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 18, MenuName = "Inventory", MenuPath = "/inventory", MenuIcon = "FaBoxOpen", ControllerName = "Inventory", ActionName = "Index", ModuleName = "Inventory", DisplayOrder = 5, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 19, MenuName = "Kitchen", MenuPath = "/kitchen", MenuIcon = "FaUtensils", ControllerName = "Kitchen", ActionName = "Index", ModuleName = "Kitchen", DisplayOrder = 6, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Menu { Id = 20, MenuName = "Category Management", MenuPath = "/categories", MenuIcon = "FaClipboardList", ControllerName = "Categories", ActionName = "Index", ModuleName = "Product Management", DisplayOrder = 4, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 21, MenuName = "Category List", ParentID = 20, MenuPath = "/categories/list", MenuIcon = "FaListUl", ControllerName = "Categories", ActionName = "List", ModuleName = "Product Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 22, MenuName = "Category Add", ParentID = 20, MenuPath = "/categories/add", MenuIcon = "FaPlus", ControllerName = "Categories", ActionName = "Add", ModuleName = "Product Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 23, MenuName = "Unit Management", MenuPath = "/units", MenuIcon = "FaRulerCombined", ControllerName = "Units", ActionName = "Index", ModuleName = "Product Management", DisplayOrder = 7, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 24, MenuName = "Unit List", ParentID = 23, MenuPath = "/units/list", MenuIcon = "FaListUl", ControllerName = "Units", ActionName = "List", ModuleName = "Product Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 25, MenuName = "Unit Add", ParentID = 23, MenuPath = "/units/add", MenuIcon = "FaPlus", ControllerName = "Units", ActionName = "Add", ModuleName = "Product Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Menu { Id = 26, MenuName = "Supplier Management", MenuPath = "/suppliers", MenuIcon = "FaTruck", ControllerName = "Suppliers", ActionName = "Index", ModuleName = "Product Management", DisplayOrder = 8, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 27, MenuName = "Supplier List", ParentID = 26, MenuPath = "/suppliers/list", MenuIcon = "FaListUl", ControllerName = "Suppliers", ActionName = "List", ModuleName = "Product Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 28, MenuName = "Supplier Add", ParentID = 26, MenuPath = "/suppliers/add", MenuIcon = "FaPlus", ControllerName = "Suppliers", ActionName = "Add", ModuleName = "Product Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Menu { Id = 29, MenuName = "Manufacturer Management", MenuPath = "/manufacturers", MenuIcon = "FaIndustry", ControllerName = "Manufacturers", ActionName = "Index", ModuleName = "Product Management", DisplayOrder = 9, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 30, MenuName = "Manufacturer List", ParentID = 29, MenuPath = "/manufacturers/list", MenuIcon = "FaListUl", ControllerName = "Manufacturers", ActionName = "List", ModuleName = "Product Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 31, MenuName = "Manufacturer Add", ParentID = 29, MenuPath = "/manufacturers/add", MenuIcon = "FaPlus", ControllerName = "Manufacturers", ActionName = "Add", ModuleName = "Product Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Menu { Id = 32, MenuName = "Product Management", MenuPath = "/products", MenuIcon = "FaBoxOpen", ControllerName = "Products", ActionName = "Index", ModuleName = "Product Management", DisplayOrder = 10, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 33, MenuName = "Product List", ParentID = 32, MenuPath = "/products/list", MenuIcon = "FaListUl", ControllerName = "Products", ActionName = "List", ModuleName = "Product Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 34, MenuName = "Product Add", ParentID = 32, MenuPath = "/products/add", MenuIcon = "FaPlus", ControllerName = "Products", ActionName = "Add", ModuleName = "Product Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Customer Management
                    new Menu { Id = 35, MenuName = "Customer Management", MenuPath = "/customers", MenuIcon = "FaUserFriends", ControllerName = "Customers", ActionName = "Index", ModuleName = "Customer Management", DisplayOrder = 11, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 36, MenuName = "Customer List", ParentID = 35, MenuPath = "/customers/list", MenuIcon = "FaListUl", ControllerName = "Customers", ActionName = "List", ModuleName = "Customer Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 37, MenuName = "Customer Add", ParentID = 35, MenuPath = "/customers/add", MenuIcon = "FaPlus", ControllerName = "Customers", ActionName = "Add", ModuleName = "Customer Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Staff Management
                    new Menu { Id = 38, MenuName = "Staff Management", MenuPath = "/staff", MenuIcon = "FaUserTie", ControllerName = "Staff", ActionName = "Index", ModuleName = "Staff Management", DisplayOrder = 12, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 39, MenuName = "Staff List", ParentID = 38, MenuPath = "/staff/list", MenuIcon = "FaListUl", ControllerName = "Staff", ActionName = "List", ModuleName = "Staff Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 40, MenuName = "Staff Add", ParentID = 38, MenuPath = "/staff/add", MenuIcon = "FaPlus", ControllerName = "Staff", ActionName = "Add", ModuleName = "Staff Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Inventory Management
                    new Menu { Id = 41, MenuName = "Inventory Management", MenuPath = "/inventory", MenuIcon = "FaBoxOpen", ControllerName = "Inventory", ActionName = "Index", ModuleName = "Inventory Management", DisplayOrder = 13, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 42, MenuName = "Inventory List", ParentID = 41, MenuPath = "/inventory/list", MenuIcon = "FaListUl", ControllerName = "Inventory", ActionName = "List", ModuleName = "Inventory Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 43, MenuName = "Alerts", ParentID = 41, MenuPath = "/alerts", MenuIcon = "FaBell", ControllerName = "Alerts", ActionName = "Index", ModuleName = "Inventory Management", DisplayOrder = 3, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Stock Transaction Management
                    new Menu { Id = 44, MenuName = "Stock Transaction Management", MenuPath = "/stock-transactions", MenuIcon = "FaExchangeAlt", ControllerName = "StockTransactions", ActionName = "Index", ModuleName = "Stock Management", DisplayOrder = 14, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 45, MenuName = "Stock Transaction List", ParentID = 44, MenuPath = "/stock-transactions/list", MenuIcon = "FaListUl", ControllerName = "StockTransactions", ActionName = "List", ModuleName = "Stock Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 46, MenuName = "Stock Transaction Add", ParentID = 44, MenuPath = "/stock-transactions/add", MenuIcon = "FaPlus", ControllerName = "StockTransactions", ActionName = "Add", ModuleName = "Stock Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Ingredient Management
                    new Menu { Id = 47, MenuName = "Ingredient Management", MenuPath = "/ingredients", MenuIcon = "FaLeaf", ControllerName = "Ingredients", ActionName = "Index", ModuleName = "Ingredient Management", DisplayOrder = 15, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 48, MenuName = "Ingredient List", ParentID = 47, MenuPath = "/ingredients/list", MenuIcon = "FaListUl", ControllerName = "Ingredients", ActionName = "List", ModuleName = "Ingredient Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 49, MenuName = "Ingredient Add", ParentID = 47, MenuPath = "/ingredients/add", MenuIcon = "FaPlus", ControllerName = "Ingredients", ActionName = "Add", ModuleName = "Ingredient Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Product Ingredient Management
                    new Menu { Id = 50, MenuName = "Product Ingredient Management", MenuPath = "/product-ingredients", MenuIcon = "FaBlender", ControllerName = "ProductIngredients", ActionName = "Index", ModuleName = "Product Ingredient Management", DisplayOrder = 16, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 51, MenuName = "Product Ingredient List", ParentID = 50, MenuPath = "/product-ingredients/list", MenuIcon = "FaListUl", ControllerName = "ProductIngredients", ActionName = "List", ModuleName = "Product Ingredient Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 52, MenuName = "Product Ingredient Add", ParentID = 50, MenuPath = "/product-ingredients/add", MenuIcon = "FaPlus", ControllerName = "ProductIngredients", ActionName = "Add", ModuleName = "Product Ingredient Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Order Management
                    new Menu { Id = 53, MenuName = "Order Management", MenuPath = "/orders", MenuIcon = "FaShoppingCart", ControllerName = "Orders", ActionName = "Index", ModuleName = "Order Management", DisplayOrder = 17, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 54, MenuName = "Order List", ParentID = 53, MenuPath = "/orders/list", MenuIcon = "FaListUl", ControllerName = "Orders", ActionName = "List", ModuleName = "Order Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 55, MenuName = "Order Add", ParentID = 53, MenuPath = "/orders/add", MenuIcon = "FaPlus", ControllerName = "Orders", ActionName = "Add", ModuleName = "Order Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Dining Table Management
                    new Menu { Id = 56, MenuName = "Dining Table Management", MenuPath = "/dining-tables", MenuIcon = "FaTable", ControllerName = "DiningTables", ActionName = "Index", ModuleName = "Dining Table Management", DisplayOrder = 18, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 57, MenuName = "Dining Table List", ParentID = 56, MenuPath = "/dining-tables/list", MenuIcon = "FaListUl", ControllerName = "DiningTables", ActionName = "List", ModuleName = "Dining Table Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 58, MenuName = "Dining Table Add", ParentID = 56, MenuPath = "/dining-tables/add", MenuIcon = "FaPlus", ControllerName = "DiningTables", ActionName = "Add", ModuleName = "Dining Table Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Promotions Management
                    new Menu { Id = 59, MenuName = "Promotions Management", MenuPath = "/promotions", MenuIcon = "FaTags", ControllerName = "Promotions", ActionName = "Index", ModuleName = "Promotions Management", DisplayOrder = 19, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 60, MenuName = "Promotion List", ParentID = 59, MenuPath = "/promotions/list", MenuIcon = "FaListUl", ControllerName = "Promotions", ActionName = "List", ModuleName = "Promotions Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 61, MenuName = "Promotion Add", ParentID = 59, MenuPath = "/promotions/add", MenuIcon = "FaPlus", ControllerName = "Promotions", ActionName = "Add", ModuleName = "Promotions Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Purchase Management
                    new Menu { Id = 62, MenuName = "Purchase Management", MenuPath = "/purchases", MenuIcon = "FaShoppingCart", ControllerName = "Purchases", ActionName = "Index", ModuleName = "Purchase Management", DisplayOrder = 20, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 63, MenuName = "Purchase List", ParentID = 62, MenuPath = "/purchases/list", MenuIcon = "FaListUl", ControllerName = "Purchases", ActionName = "List", ModuleName = "Purchase Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 64, MenuName = "Purchase Add", ParentID = 62, MenuPath = "/purchases/create", MenuIcon = "FaPlus", ControllerName = "Purchases", ActionName = "Create", ModuleName = "Purchase Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 65, MenuName = "Purchase Edit", ParentID = 62, MenuPath = "/purchases/edit", MenuIcon = "FaEdit", ControllerName = "Purchases", ActionName = "Edit", ModuleName = "Purchase Management", DisplayOrder = 3, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 66, MenuName = "Purchase Detail", ParentID = 62, MenuPath = "/purchases/detail", MenuIcon = "FaInfoCircle", ControllerName = "Purchases", ActionName = "Detail", ModuleName = "Purchase Management", DisplayOrder = 4, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Sales Management
                    new Menu { Id = 67, MenuName = "Sales Management", MenuPath = "/sales", MenuIcon = "FaChartLine", ControllerName = "Sales", ActionName = "Index", ModuleName = "Sale Management", DisplayOrder = 21, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 68, MenuName = "Sales List", ParentID = 67, MenuPath = "/sales/list", MenuIcon = "FaListUl", ControllerName = "Sales", ActionName = "List", ModuleName = "Sale Management", DisplayOrder = 1, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 69, MenuName = "Sales Add", ParentID = 67, MenuPath = "/sales/add", MenuIcon = "FaPlus", ControllerName = "Sales", ActionName = "Add", ModuleName = "Sale Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 70, MenuName = "Sales Edit", ParentID = 67, MenuPath = "/sales/edit", MenuIcon = "FaEdit", ControllerName = "Sales", ActionName = "Edit", ModuleName = "Sale Management", DisplayOrder = 3, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Menu { Id = 71, MenuName = "Sale Detail", ParentID = 67, MenuPath = "/sales/detail", MenuIcon = "FaInfoCircle", ControllerName = "Sales", ActionName = "Detail", ModuleName = "Sale Management", DisplayOrder = 4, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
                );
        }
    }
}
