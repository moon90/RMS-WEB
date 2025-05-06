using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
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

        public async Task<RolePermission> GetRolePermissionByIdAsync(int rolePermissionId)
        {
            return await _context.RolePermissions.FindAsync(rolePermissionId);
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleID == roleId)
                .ToListAsync();
        }

        public async Task<List<Permission>> GetPermissionsForRoleAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleID == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task AddRolePermissionAsync(RolePermission rolePermission)
        {
            await _context.RolePermissions.AddAsync(rolePermission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRolePermissionAsync(RolePermission rolePermission)
        {
            _context.RolePermissions.Update(rolePermission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRolePermissionAsync(int rolePermissionId)
        {
            var rolePermission = await _context.RolePermissions.FindAsync(rolePermissionId);
            if (rolePermission != null)
            {
                _context.RolePermissions.Remove(rolePermission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsPermissionAssignedToRoleAsync(int roleId, int permissionId)
        {
            return await _context.RolePermissions
                .AnyAsync(rp => rp.RoleID == roleId && rp.PermissionID == permissionId);
        }

        public async Task AssignPermissionToRoleAsync(int roleId, int permissionId, int? sortingOrder = null)
        {
            if (!await IsPermissionAssignedToRoleAsync(roleId, permissionId))
            {
                var rolePermission = new RolePermission
                {
                    RoleID = roleId,
                    PermissionID = permissionId
                };
                await _context.RolePermissions.AddAsync(rolePermission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnassignPermissionFromRoleAsync(int roleId, int permissionId)
        {
            var rolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleID == roleId && rp.PermissionID == permissionId);

            if (rolePermission != null)
            {
                _context.RolePermissions.Remove(rolePermission);
                await _context.SaveChangesAsync();
            }
        }
    }
}
