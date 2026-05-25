using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Infrastructure.IRepositories
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>
    {
        Task<RolePermission?> GetRolePermissionByIdAsync(int rolePermissionId);
        Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(int roleId);
        Task<List<Permission>> GetPermissionsForRoleAsync(int roleId);
        Task AddRolePermissionAsync(RolePermission rolePermission);
        Task UpdateRolePermissionAsync(RolePermission rolePermission);
        Task DeleteRolePermissionAsync(int rolePermissionId);

        // New methods for permission assignment
        Task<bool> IsPermissionAssignedToRoleAsync(int roleId, int permissionId);
        Task AssignPermissionToRoleAsync(int roleId, int permissionId, int? sortingOrder = null);
        Task AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
        Task UnassignPermissionFromRoleAsync(int roleId, int permissionId);
    }
}
