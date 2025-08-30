using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Infrastructure.Interfaces;
using RMS.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly RestaurantDbContext _context;

        public RolePermissionRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<RolePermission?> GetRolePermissionByIdAsync(int rolePermissionId)
        {
            try
            {
                return await _context.RolePermissions.FindAsync(rolePermissionId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving role permission by ID: {ex.Message}");
                throw; 
            }
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(int roleId)
        {
            try
            {
                return await _context.RolePermissions
                    .Where(rp => rp.RoleID == roleId)
                    .Include(rp => rp.Permission)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving role permissions by role ID: {ex.Message}");
                throw; 
            }
        }

        public async Task<List<Permission>> GetPermissionsForRoleAsync(int roleId)
        {
            try
            {
                return await _context.RolePermissions
                    .Where(rp => rp.RoleID == roleId)
                    .Select(rp => rp.Permission)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving permissions for role: {ex.Message}");
                throw; 
            }
        }

        public async Task AddRolePermissionAsync(RolePermission rolePermission)
        {
            try
            {
                await _context.RolePermissions.AddAsync(rolePermission);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding role permission: {ex.Message}");
                throw; 
            }
        }

        public async Task UpdateRolePermissionAsync(RolePermission rolePermission)
        {
            try
            {
                _context.RolePermissions.Update(rolePermission);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating role permission: {ex.Message}");
                throw; 
            }
        }

        public async Task DeleteRolePermissionAsync(int rolePermissionId)
        {
            try
            {
                var rolePermission = await _context.RolePermissions.FindAsync(rolePermissionId);
                if (rolePermission != null)
                {
                    _context.RolePermissions.Remove(rolePermission);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting role permission: {ex.Message}");
                throw; 
            }
        }

        public async Task<bool> IsPermissionAssignedToRoleAsync(int roleId, int permissionId)
        {
            try
            {
                return await _context.RolePermissions
                    .AnyAsync(rp => rp.RoleID == roleId && rp.PermissionID == permissionId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if permission is assigned to role: {ex.Message}");
                throw; 
            }
        }

        public async Task AssignPermissionToRoleAsync(int roleId, int permissionId, int? sortingOrder = null)
        {
            try
            {
                if (!await IsPermissionAssignedToRoleAsync(roleId, permissionId))
                {
                    var rolePermission = new RolePermission
                    {
                        RoleID = roleId,
                        PermissionID = permissionId,
                        SortingOrder = sortingOrder
                    };
                    await _context.RolePermissions.AddAsync(rolePermission);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning permission to role: {ex.Message}");
                throw; 
            }
        }

        public async Task AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Remove all existing permissions for the role
                var existingPermissions = await _context.RolePermissions
                    .Where(rp => rp.RoleID == roleId)
                    .ToListAsync();

                _context.RolePermissions.RemoveRange(existingPermissions);

                // Add the new set of permissions
                var newPermissions = permissionIds.Select(permissionId => new RolePermission
                {
                    RoleID = roleId,
                    PermissionID = permissionId,
                    AssignedAt = DateTime.UtcNow,
                    AssignedBy = "System" // Or get the current user
                });

                await _context.RolePermissions.AddRangeAsync(newPermissions);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error assigning permissions to role: {ex.Message}");
                throw;
            }
        }

        public async Task UnassignPermissionFromRoleAsync(int roleId, int permissionId)
        {
            try
            {
                var rolePermission = await _context.RolePermissions
                    .FirstOrDefaultAsync(rp => rp.RoleID == roleId && rp.PermissionID == permissionId);

                if (rolePermission != null)
                {
                    _context.RolePermissions.Remove(rolePermission);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unassigning permission from role: {ex.Message}");
                throw; 
            }
        }
    }
}