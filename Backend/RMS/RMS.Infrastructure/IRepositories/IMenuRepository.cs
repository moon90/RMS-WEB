using RMS.Domain.Interfaces;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.IRepositories
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        Task<Menu?> GetMenuByIdAsync(int menuId);
        Task<Menu?> GetMenuByNameAsync(string menuName);
        Task<IEnumerable<Menu>> GetAllMenusAsync();
        Task<(IEnumerable<Menu> Menus, int TotalCount)> GetAllMenusAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection);
        Task<IEnumerable<Menu>> GetMenusByParentIdAsync(int? parentId);
        Task AddMenuAsync(Menu menu);
        Task UpdateMenuAsync(Menu menu);
        Task DeleteMenuAsync(int menuId);
        Task<bool> MenuExistsAsync(int menuId);
    }
}
