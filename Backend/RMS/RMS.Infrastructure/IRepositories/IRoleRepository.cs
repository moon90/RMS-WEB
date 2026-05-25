using RMS.Domain.Interfaces;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.IRepositories
{
    public interface IRoleRepository : IBaseRepository<Role>
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
