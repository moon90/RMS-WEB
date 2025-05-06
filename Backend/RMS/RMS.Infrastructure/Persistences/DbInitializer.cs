using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Persistences
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(RestaurantDbContext context)
        {
            await context.Database.MigrateAsync();

            // Seed Role
            if (!context.Roles.Any())
            {
                context.Roles.Add(new Role
                {
                    RoleName = "Admin",
                    Description = "Administrator",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow
                });
            }

            // Seed Admin User
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                context.Users.Add(new User
                {
                    UserName = "admin",
                    FullName = "System Administrator",
                    Email = "admin@example.com",
                    Phone = "0000000000",
                    PasswordHash = "c4vD3op8WBxFFjk_XPZoHA", // Replace with actual hashed password
                    PasswordSalt = "qtixrauL4wM-8gdAhr6rAA", // Replace with actual password salt
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                });
            }

            if (!context.Users.Any(u => u.UserName == "manager"))
            {
                context.Users.Add(new User
                {
                    UserName = "manager",
                    PasswordHash = "d4QTV4pwUJ-pwL2B2Y4V_w", // Replace with actual hashed password
                    PasswordSalt = "cZ7UtVxlTYIEb97pOqfoBQ", // Replace with actual password salt
                    FullName = "Manager User",
                    Email = "manager@example.com",
                    Phone = "0987654321",
                    RefreshToken = null,
                    RefreshTokenExpiry = null,
                    Status = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                });
            }

            if (!context.Users.Any(u => u.UserName == "user"))
            {
                context.Users.Add(new User
                {
                    UserName = "user",
                    PasswordHash = "6Fn94S0iWXBrFbYv5v4Yxg", // Replace with actual hashed password
                    PasswordSalt = "6Fn94S0iWXBrFbYv5v4Yxg", // Replace with actual password salt
                    FullName = "User",
                    Email = "user@example.com",
                    Phone = "0987654321",
                    RefreshToken = null,
                    RefreshTokenExpiry = null,
                    Status = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                });
            }

            if (!context.UserRoles.Any())
            {
                context.UserRoles.AddRange(
                    new UserRole
                    {
                        Id = 1,
                        UserID = 1, // Admin user
                        RoleID = 1,  // Admin role
                        AssignedBy = "System", // ✅ Required field
                        AssignedAt = DateTime.UtcNow
                    },
                    new UserRole
                    {
                        Id = 2,
                        UserID = 2, // Manager user
                        RoleID = 2,  // Manager role
                        AssignedBy = "System", // ✅ Required field
                        AssignedAt = DateTime.UtcNow
                    }
                );
            }

            // Seed Menus
            if (!context.Menus.Any())
            {
                context.Menus.AddRange(
                    new Menu
                    {
                        MenuName = "Dashboard",
                        MenuPath = "/dashboard",
                        MenuIcon = "dashboard",
                        DisplayOrder = 1,
                        Status = true,
                        CreatedBy = "system",
                        CreatedDate = DateTime.UtcNow
                    },
                    new Menu
                    {
                        MenuName = "Users",
                        MenuPath = "/users",
                        MenuIcon = "users",
                        DisplayOrder = 2,
                        Status = true,
                        CreatedBy = "system",
                        CreatedDate = DateTime.UtcNow
                    }
                );
            }

            // Seed Permissions
            if (!context.Permissions.Any())
            {
                context.Permissions.AddRange(
                    new Permission
                    {
                        PermissionName = "View Users",
                        PermissionKey = "VIEW_USERS",
                        Status = true,
                        CreatedBy = "system",
                        CreatedDate = DateTime.UtcNow
                    },
                    new Permission
                    {
                        PermissionName = "Edit Users",
                        PermissionKey = "EDIT_USERS",
                        Status = true,
                        CreatedBy = "system",
                        CreatedDate = DateTime.UtcNow
                    }
                );
            }

            // Seed RolePermissions
            if (!context.RolePermissions.Any())
            {
                context.RolePermissions.AddRange(
                    new RolePermission
                    {
                        RoleID = 1,
                        PermissionID = 1,
                        AssignedBy = "System", // ✅ Required field
                        AssignedAt = DateTime.UtcNow
                    },
                    new RolePermission
                    {
                        RoleID = 1,
                        PermissionID = 2,
                        AssignedBy = "System", // ✅ Required field
                        AssignedAt = DateTime.UtcNow
                    }
                );
            }

            // Seed RoleMenus
            if (!context.RoleMenus.Any())
            {
                context.RoleMenus.AddRange(
                    new RoleMenu
                    {
                        RoleID = 1,
                        MenuID = 1,
                        CanView = true,
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = false,
                        AssignedBy = "System", // ✅ Required field
                        AssignedAt = DateTime.UtcNow
                    },
                    new RoleMenu
                    {
                        RoleID = 1,
                        MenuID = 2,
                        CanView = true,
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        AssignedBy = "System", // ✅ Required field
                        AssignedAt = DateTime.UtcNow
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }

}
