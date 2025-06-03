using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Interfaces
{
    public interface IMenuRepository
    {
        Task<Menu> GetMenuByIdAsync(int menuId);
        Task<IEnumerable<Menu>> GetAllMenusAsync();
        Task<(IEnumerable<Menu> Menus, int TotalCount)> GetAllMenusAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Menu>> GetMenusByParentIdAsync(int? parentId);
        Task AddMenuAsync(Menu menu);
        Task UpdateMenuAsync(Menu menu);
        Task DeleteMenuAsync(int menuId);
        Task<bool> MenuExistsAsync(int menuId);
    }
}
