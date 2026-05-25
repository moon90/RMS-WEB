using RMS.Domain.Interfaces;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.IRepositories
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task<Permission?> GetPermissionByIdAsync(int permissionId);
        Task<Permission?> GetPermissionByNameAsync(string permissionName);
        Task<Permission?> GetPermissionByKeyAsync(string permissionKey);
        Task<(IEnumerable<Permission> Permissions, int TotalCount)> GetAllPermissionsAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task AddPermissionAsync(Permission permission);
        Task UpdatePermissionAsync(Permission permission);
        Task DeletePermissionAsync(int permissionId);
    }
}
