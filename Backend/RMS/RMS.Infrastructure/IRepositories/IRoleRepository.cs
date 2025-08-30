using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task<(IEnumerable<Role> Roles, int TotalCount)> GetAllRolesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection);
        Task AddRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int roleId);
        Task<bool> RoleExistsAsync(int roleId);
    }
}
