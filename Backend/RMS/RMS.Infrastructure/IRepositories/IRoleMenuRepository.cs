using RMS.Domain.Interfaces;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.IRepositories
{
    public interface IRoleMenuRepository : IBaseRepository<RoleMenu>
    {
        Task<RoleMenu?> GetRoleMenuByIdAsync(int roleMenuId);
        Task<IEnumerable<RoleMenu>> GetRoleMenusByRoleIdAsync(int roleId);
        Task<IEnumerable<RoleMenu>> GetRoleMenusByMenuIdAsync(int menuId);
        Task AddRoleMenuAsync(RoleMenu roleMenu);
        Task UpdateRoleMenuAsync(RoleMenu roleMenu);
        Task DeleteRoleMenuAsync(int roleMenuId);

        // New methods for menu assignment
        Task<bool> IsMenuAssignedToRoleAsync(int roleId, int menuId);
        Task AssignMenuToRoleAsync(int roleId, int menuId, bool canView, bool canAdd, bool canEdit, bool canDelete);
        Task UnassignMenuFromRoleAsync(int roleId, int menuId);
        Task<RoleMenu?> GetRoleMenuByRoleIdAndMenuIdAsync(int roleId, int menuId);
    }
}
