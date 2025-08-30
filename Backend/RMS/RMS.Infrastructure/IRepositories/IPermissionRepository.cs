using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Interfaces
{
    public interface IPermissionRepository
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
