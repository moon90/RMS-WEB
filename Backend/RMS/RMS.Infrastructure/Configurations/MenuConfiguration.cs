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
                    new Menu { Id = 34, MenuName = "Product Add", ParentID = 32, MenuPath = "/products/add", MenuIcon = "FaPlus", ControllerName = "Products", ActionName = "Add", ModuleName = "Product Management", DisplayOrder = 2, Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
                );
        }
    }
}
