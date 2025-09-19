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
                    new Permission { Id = 85, PermissionName = "Toggle Role Status", PermissionKey = "ROLE_TOGGLE_STATUS", ControllerName = "Roles", ActionName = "UpdateRoleStatus", ModuleName = "Role Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    new Permission { Id = 31, PermissionName = "View Categories", PermissionKey = "CATEGORY_VIEW", ControllerName = "Categories", ActionName = "GetAllCategories", ModuleName = "Category Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 32, PermissionName = "Create Category", PermissionKey = "CATEGORY_CREATE", ControllerName = "Categories", ActionName = "Create", ModuleName = "Category Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 33, PermissionName = "Update Category", PermissionKey = "CATEGORY_UPDATE", ControllerName = "Categories", ActionName = "Update", ModuleName = "Category Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 34, PermissionName = "Delete Category", PermissionKey = "CATEGORY_DELETE", ControllerName = "Categories", ActionName = "Delete", ModuleName = "Category Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 86, PermissionName = "Toggle Category Status", PermissionKey = "CATEGORY_TOGGLE_STATUS", ControllerName = "Categories", ActionName = "UpdateStatus", ModuleName = "Category Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
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
                    new Permission { Id = 50, PermissionName = "Delete Product", PermissionKey = "PRODUCT_DELETE", ControllerName = "Products", ActionName = "DeleteProduct", ModuleName = "Product Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Customer Permissions
                    new Permission { Id = 51, PermissionName = "View Customers", PermissionKey = "CUSTOMER_VIEW", ControllerName = "Customers", ActionName = "GetAllCustomers", ModuleName = "Customer Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 52, PermissionName = "Create Customer", PermissionKey = "CUSTOMER_CREATE", ControllerName = "Customers", ActionName = "CreateCustomer", ModuleName = "Customer Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 53, PermissionName = "Update Customer", PermissionKey = "CUSTOMER_UPDATE", ControllerName = "Customers", ActionName = "UpdateCustomer", ModuleName = "Customer Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 54, PermissionName = "Delete Customer", PermissionKey = "CUSTOMER_DELETE", ControllerName = "Customers", ActionName = "DeleteCustomer", ModuleName = "Customer Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Staff Permissions
                    new Permission { Id = 55, PermissionName = "View Staff", PermissionKey = "STAFF_VIEW", ControllerName = "Staff", ActionName = "GetAllStaff", ModuleName = "Staff Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 56, PermissionName = "Create Staff", PermissionKey = "STAFF_CREATE", ControllerName = "Staff", ActionName = "CreateStaff", ModuleName = "Staff Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 57, PermissionName = "Update Staff", PermissionKey = "STAFF_UPDATE", ControllerName = "Staff", ActionName = "UpdateStaff", ModuleName = "Staff Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 58, PermissionName = "Delete Staff", PermissionKey = "STAFF_DELETE", ControllerName = "Staff", ActionName = "DeleteStaff", ModuleName = "Staff Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Inventory Permissions
                    new Permission { Id = 59, PermissionName = "View Inventory", PermissionKey = "INVENTORY_VIEW", ControllerName = "Inventory", ActionName = "GetAll", ModuleName = "Inventory Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 60, PermissionName = "Create Inventory", PermissionKey = "INVENTORY_CREATE", ControllerName = "Inventory", ActionName = "Create", ModuleName = "Inventory Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 61, PermissionName = "Update Inventory", PermissionKey = "INVENTORY_UPDATE", ControllerName = "Inventory", ActionName = "Update", ModuleName = "Inventory Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 62, PermissionName = "Delete Inventory", PermissionKey = "INVENTORY_DELETE", ControllerName = "Inventory", ActionName = "Delete", ModuleName = "Inventory Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 75, PermissionName = "View Alerts", PermissionKey = "ALERT_VIEW", ControllerName = "Alerts", ActionName = "GetAlerts", ModuleName = "Alert Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 99, PermissionName = "Acknowledge Alert", PermissionKey = "ALERT_ACKNOWLEDGE", ControllerName = "Alerts", ActionName = "AcknowledgeAlert", ModuleName = "Alert Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Stock Transaction Permissions
                    new Permission { Id = 63, PermissionName = "View Stock Transactions", PermissionKey = "STOCK_TRANSACTION_VIEW", ControllerName = "StockTransactions", ActionName = "GetAll", ModuleName = "Stock Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 64, PermissionName = "Create Stock Transaction", PermissionKey = "STOCK_TRANSACTION_CREATE", ControllerName = "StockTransactions", ActionName = "Create", ModuleName = "Stock Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 65, PermissionName = "Update Stock Transaction", PermissionKey = "STOCK_TRANSACTION_UPDATE", ControllerName = "StockTransactions", ActionName = "Update", ModuleName = "Stock Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 66, PermissionName = "Delete Stock Transaction", PermissionKey = "STOCK_TRANSACTION_DELETE", ControllerName = "StockTransactions", ActionName = "Delete", ModuleName = "Stock Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Ingredient Permissions
                    new Permission { Id = 67, PermissionName = "View Ingredients", PermissionKey = "INGREDIENT_VIEW", ControllerName = "Ingredients", ActionName = "GetAll", ModuleName = "Ingredient Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 68, PermissionName = "Create Ingredient", PermissionKey = "INGREDIENT_CREATE", ControllerName = "Ingredients", ActionName = "Create", ModuleName = "Ingredient Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 69, PermissionName = "Update Ingredient", PermissionKey = "INGREDIENT_UPDATE", ControllerName = "Ingredients", ActionName = "Update", ModuleName = "Ingredient Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 70, PermissionName = "Delete Ingredient", PermissionKey = "INGREDIENT_DELETE", ControllerName = "Ingredients", ActionName = "Delete", ModuleName = "Ingredient Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Product Ingredient Permissions
                    new Permission { Id = 71, PermissionName = "View Product Ingredients", PermissionKey = "PRODUCT_INGREDIENT_VIEW", ControllerName = "ProductIngredients", ActionName = "GetAll", ModuleName = "Product Ingredient Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 72, PermissionName = "Create Product Ingredient", PermissionKey = "PRODUCT_INGREDIENT_CREATE", ControllerName = "ProductIngredients", ActionName = "Create", ModuleName = "Product Ingredient Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 73, PermissionName = "Update Product Ingredient", PermissionKey = "PRODUCT_INGREDIENT_UPDATE", ControllerName = "ProductIngredients", ActionName = "Update", ModuleName = "Product Ingredient Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 74, PermissionName = "Delete Product Ingredient", PermissionKey = "PRODUCT_INGREDIENT_DELETE", ControllerName = "ProductIngredients", ActionName = "Delete", ModuleName = "Product Ingredient Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Order Permissions
                    new Permission { Id = 76, PermissionName = "View Orders", PermissionKey = "ORDER_VIEW", ControllerName = "Orders", ActionName = "GetAllOrders", ModuleName = "Order Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 77, PermissionName = "Create Order", PermissionKey = "ORDER_CREATE", ControllerName = "Orders", ActionName = "CreateOrder", ModuleName = "Order Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 78, PermissionName = "Update Order", PermissionKey = "ORDER_UPDATE", ControllerName = "Orders", ActionName = "UpdateOrder", ModuleName = "Order Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 79, PermissionName = "Delete Order", PermissionKey = "ORDER_DELETE", ControllerName = "Orders", ActionName = "DeleteOrder", ModuleName = "Order Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Dining Table Permissions
                    new Permission { Id = 80, PermissionName = "View Dining Tables", PermissionKey = "DINING_TABLE_VIEW", ControllerName = "DiningTables", ActionName = "GetAllDiningTables", ModuleName = "Dining Table Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 81, PermissionName = "Create Dining Table", PermissionKey = "DINING_TABLE_CREATE", ControllerName = "DiningTables", ActionName = "CreateDiningTable", ModuleName = "Dining Table Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 82, PermissionName = "Update Dining Table", PermissionKey = "DINING_TABLE_UPDATE", ControllerName = "DiningTables", ActionName = "UpdateDiningTable", ModuleName = "Dining Table Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 83, PermissionName = "Delete Dining Table", PermissionKey = "DINING_TABLE_DELETE", ControllerName = "DiningTables", ActionName = "DeleteDiningTable", ModuleName = "Dining Table Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 84, PermissionName = "Toggle User Status", PermissionKey = "USER_TOGGLE_STATUS", ControllerName = "Users", ActionName = "UpdateUserStatus", ModuleName = "User Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Promotion Permissions
                    new Permission { Id = 87, PermissionName = "View Promotions", PermissionKey = "PROMOTION_VIEW", ControllerName = "Promotions", ActionName = "GetAllPromotions", ModuleName = "Promotions Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 88, PermissionName = "Create Promotion", PermissionKey = "PROMOTION_CREATE", ControllerName = "Promotions", ActionName = "CreatePromotion", ModuleName = "Promotions Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 89, PermissionName = "Update Promotion", PermissionKey = "PROMOTION_UPDATE", ControllerName = "Promotions", ActionName = "UpdatePromotion", ModuleName = "Promotions Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 90, PermissionName = "Delete Promotion", PermissionKey = "PROMOTION_DELETE", ControllerName = "Promotions", ActionName = "DeletePromotion", ModuleName = "Promotions Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Purchase Permissions
                    new Permission { Id = 91, PermissionName = "View Purchases", PermissionKey = "PURCHASE_VIEW", ControllerName = "Purchases", ActionName = "GetAllPurchases", ModuleName = "Purchase Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 92, PermissionName = "Create Purchase", PermissionKey = "PURCHASE_CREATE", ControllerName = "Purchases", ActionName = "CreatePurchase", ModuleName = "Purchase Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 93, PermissionName = "Update Purchase", PermissionKey = "PURCHASE_UPDATE", ControllerName = "Purchases", ActionName = "UpdatePurchase", ModuleName = "Purchase Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 94, PermissionName = "Delete Purchase", PermissionKey = "PURCHASE_DELETE", ControllerName = "Purchases", ActionName = "DeletePurchase", ModuleName = "Purchase Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                    // Sale Permissions
                    new Permission { Id = 95, PermissionName = "View Sales", PermissionKey = "SALE_VIEW", ControllerName = "Sales", ActionName = "GetAllSales", ModuleName = "Sale Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 96, PermissionName = "Create Sale", PermissionKey = "SALE_CREATE", ControllerName = "Sales", ActionName = "CreateSale", ModuleName = "Sale Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 97, PermissionName = "Update Sale", PermissionKey = "SALE_UPDATE", ControllerName = "Sales", ActionName = "UpdateSale", ModuleName = "Sale Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Permission { Id = 98, PermissionName = "Delete Sale", PermissionKey = "SALE_DELETE", ControllerName = "Sales", ActionName = "DeleteSale", ModuleName = "Sale Management", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
                );
        }
    }
}
