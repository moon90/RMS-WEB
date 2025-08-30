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
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.PermissionName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PermissionKey).HasMaxLength(100);

            // Seed
            builder.HasData(
                    new Permission { Id = 1, PermissionName = "View Users", PermissionKey = "USER_VIEW", ControllerName = "Users", ActionName = "GetAllUsers", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 2, PermissionName = "Create User", PermissionKey = "USER_CREATE", ControllerName = "Users", ActionName = "CreateUser", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 3, PermissionName = "Update User", PermissionKey = "USER_UPDATE", ControllerName = "Users", ActionName = "UpdateUser", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 4, PermissionName = "Delete User", PermissionKey = "USER_DELETE", ControllerName = "Users", ActionName = "DeleteUser", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 5, PermissionName = "Assign Role to User", PermissionKey = "USER_ASSIGN_ROLE", ControllerName = "Users", ActionName = "AssignRoleToUser", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 6, PermissionName = "Unassign Role from User", PermissionKey = "USER_UNASSIGN_ROLE", ControllerName = "Users", ActionName = "UnassignRoleFromUser", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 7, PermissionName = "Assign Multiple Roles to User", PermissionKey = "USER_ASSIGN_ROLES", ControllerName = "Users", ActionName = "AssignRolesToUser", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 8, PermissionName = "Unassign Multiple Roles from User", PermissionKey = "USER_UNASSIGN_ROLES", ControllerName = "Users", ActionName = "UnassignRolesFromUser", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 9, PermissionName = "Upload User Profile Picture", PermissionKey = "USER_UPLOAD_PROFILE_PICTURE", ControllerName = "Users", ActionName = "UploadProfilePicture", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 10, PermissionName = "View User Menu Permissions", PermissionKey = "USER_VIEW_MENU_PERMISSIONS", ControllerName = "Users", ActionName = "GetMenuPermissions", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Permission { Id = 11, PermissionName = "View Menus", PermissionKey = "MENU_VIEW", ControllerName = "Menus", ActionName = "GetAllMenus", ModuleName = "Menu Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 12, PermissionName = "Create Menu", PermissionKey = "MENU_CREATE", ControllerName = "Menus", ActionName = "CreateMenu", ModuleName = "Menu Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 13, PermissionName = "Update Menu", PermissionKey = "MENU_UPDATE", ControllerName = "Menus", ActionName = "UpdateMenu", ModuleName = "Menu Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 14, PermissionName = "Delete Menu", PermissionKey = "MENU_DELETE", ControllerName = "Menus", ActionName = "DeleteMenu", ModuleName = "Menu Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 15, PermissionName = "Assign Menu to Role", PermissionKey = "MENU_ASSIGN_ROLE", ControllerName = "Menus", ActionName = "AssignMenuToRole", ModuleName = "Menu Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 16, PermissionName = "Unassign Menu from Role", PermissionKey = "MENU_UNASSIGN_ROLE", ControllerName = "Menus", ActionName = "UnassignMenuFromRole", ModuleName = "Menu Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Permission { Id = 17, PermissionName = "View Audit Logs", PermissionKey = "AUDIT_LOG_VIEW", ControllerName = "AuditLog", ActionName = "GetAll", ModuleName = "System", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Permission { Id = 18, PermissionName = "View Permissions", PermissionKey = "PERMISSION_VIEW", ControllerName = "Permission", ActionName = "GetAll", ModuleName = "Permission Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 19, PermissionName = "Create Permission", PermissionKey = "PERMISSION_CREATE", ControllerName = "Permission", ActionName = "Create", ModuleName = "Permission Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 20, PermissionName = "Update Permission", PermissionKey = "PERMISSION_UPDATE", ControllerName = "Permission", ActionName = "Update", ModuleName = "Permission Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 21, PermissionName = "Delete Permission", PermissionKey = "PERMISSION_DELETE", ControllerName = "Permission", ActionName = "Delete", ModuleName = "Permission Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Permission { Id = 22, PermissionName = "View Roles", PermissionKey = "ROLE_VIEW", ControllerName = "Roles", ActionName = "GetAllRoles", ModuleName = "Role Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 23, PermissionName = "Create Role", PermissionKey = "ROLE_CREATE", ControllerName = "Roles", ActionName = "CreateRole", ModuleName = "Role Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 24, PermissionName = "Update Role", PermissionKey = "ROLE_UPDATE", ControllerName = "Roles", ActionName = "UpdateRole", ModuleName = "Role Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 25, PermissionName = "Delete Role", PermissionKey = "ROLE_DELETE", ControllerName = "Roles", ActionName = "DeleteRole", ModuleName = "Role Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 26, PermissionName = "Assign Permission to Role", PermissionKey = "ROLE_ASSIGN_PERMISSION", ControllerName = "Roles", ActionName = "AssignPermissionToRole", ModuleName = "Role Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 27, PermissionName = "Unassign Permission from Role", PermissionKey = "ROLE_UNASSIGN_PERMISSION", ControllerName = "Roles", ActionName = "UnassignPermissionFromRole", ModuleName = "Role Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Permission { Id = 28, PermissionName = "View Inventory", PermissionKey = "INVENTORY_VIEW", ControllerName = "Inventory", ActionName = "GetAll", ModuleName = "Inventory Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 29, PermissionName = "View Kitchen", PermissionKey = "KITCHEN_VIEW", ControllerName = "Kitchen", ActionName = "GetAll", ModuleName = "Kitchen Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 30, PermissionName = "View Role Menus", PermissionKey = "ROLE_VIEW_MENUS", ControllerName = "Roles", ActionName = "GetRoleMenus", ModuleName = "Role Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Permission { Id = 31, PermissionName = "View Categories", PermissionKey = "CATEGORY_VIEW", ControllerName = "Categories", ActionName = "GetAllCategories", ModuleName = "Category Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 32, PermissionName = "Create Category", PermissionKey = "CATEGORY_CREATE", ControllerName = "Categories", ActionName = "Create", ModuleName = "Category Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 33, PermissionName = "Update Category", PermissionKey = "CATEGORY_UPDATE", ControllerName = "Categories", ActionName = "Update", ModuleName = "Category Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 34, PermissionName = "Delete Category", PermissionKey = "CATEGORY_DELETE", ControllerName = "Categories", ActionName = "Delete", ModuleName = "Category Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 35, PermissionName = "View Units", PermissionKey = "UNIT_VIEW", ControllerName = "Units", ActionName = "GetAllUnits", ModuleName = "Unit Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 36, PermissionName = "Create Unit", PermissionKey = "UNIT_CREATE", ControllerName = "Units", ActionName = "CreateUnit", ModuleName = "Unit Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 37, PermissionName = "Update Unit", PermissionKey = "UNIT_UPDATE", ControllerName = "Units", ActionName = "UpdateUnit", ModuleName = "Unit Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 38, PermissionName = "Delete Unit", PermissionKey = "UNIT_DELETE", ControllerName = "Units", ActionName = "DeleteUnit", ModuleName = "Unit Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Permission { Id = 39, PermissionName = "View Suppliers", PermissionKey = "SUPPLIER_VIEW", ControllerName = "Suppliers", ActionName = "GetAllSuppliers", ModuleName = "Supplier Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 40, PermissionName = "Create Supplier", PermissionKey = "SUPPLIER_CREATE", ControllerName = "Suppliers", ActionName = "CreateSupplier", ModuleName = "Supplier Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 41, PermissionName = "Update Supplier", PermissionKey = "SUPPLIER_UPDATE", ControllerName = "Suppliers", ActionName = "UpdateSupplier", ModuleName = "Supplier Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 42, PermissionName = "Delete Supplier", PermissionKey = "SUPPLIER_DELETE", ControllerName = "Suppliers", ActionName = "DeleteSupplier", ModuleName = "Supplier Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Permission { Id = 43, PermissionName = "View Manufacturers", PermissionKey = "MANUFACTURER_VIEW", ControllerName = "Manufacturers", ActionName = "GetAllManufacturers", ModuleName = "Manufacturer Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 44, PermissionName = "Create Manufacturer", PermissionKey = "MANUFACTURER_CREATE", ControllerName = "Manufacturers", ActionName = "CreateManufacturer", ModuleName = "Manufacturer Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 45, PermissionName = "Update Manufacturer", PermissionKey = "MANUFACTURER_UPDATE", ControllerName = "Manufacturers", ActionName = "UpdateManufacturer", ModuleName = "Manufacturer Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 46, PermissionName = "Delete Manufacturer", PermissionKey = "MANUFACTURER_DELETE", ControllerName = "Manufacturers", ActionName = "DeleteManufacturer", ModuleName = "Manufacturer Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Permission { Id = 47, PermissionName = "View Products", PermissionKey = "PRODUCT_VIEW", ControllerName = "Products", ActionName = "GetAllProducts", ModuleName = "Product Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 48, PermissionName = "Create Product", PermissionKey = "PRODUCT_CREATE", ControllerName = "Products", ActionName = "CreateProduct", ModuleName = "Product Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 49, PermissionName = "Update Product", PermissionKey = "PRODUCT_UPDATE", ControllerName = "Products", ActionName = "UpdateProduct", ModuleName = "Product Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 50, PermissionName = "Delete Product", PermissionKey = "PRODUCT_DELETE", ControllerName = "Products", ActionName = "DeleteProduct", ModuleName = "Product Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
                );
        }
    }
}
