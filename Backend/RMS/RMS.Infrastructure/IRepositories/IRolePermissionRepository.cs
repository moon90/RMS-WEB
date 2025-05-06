using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Interfaces
{
    public interface IRolePermissionRepository
    {
        Task<RolePermission> GetRolePermissionByIdAsync(int rolePermissionId);
        Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(int roleId);
        Task<List<Permission>> GetPermissionsForRoleAsync(int roleId);
        Task AddRolePermissionAsync(RolePermission rolePermission);
        Task UpdateRolePermissionAsync(RolePermission rolePermission);
        Task DeleteRolePermissionAsync(int rolePermissionId);

        // New methods for permission assignment
        Task<bool> IsPermissionAssignedToRoleAsync(int roleId, int permissionId);
        Task AssignPermissionToRoleAsync(int roleId, int permissionId, int? sortingOrder = null);
        Task UnassignPermissionFromRoleAsync(int roleId, int permissionId);
    }
}
