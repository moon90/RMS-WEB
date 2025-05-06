using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Domain.DTOs.MenuDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleMenuDTOs.OutputDTOs;
using RMS.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IMenuService
    {
        Task<MenuDto> GetMenuByIdAsync(int menuId);
        Task<IEnumerable<MenuDto>> GetAllMenusAsync();
        Task<PagedResult<MenuDto>> GetAllMenusAsync(int pageNumber, int pageSize);
        Task<int> CreateMenuAsync(MenuCreateDto menuCreateDto);
        Task UpdateMenuAsync(MenuUpdateDto menuUpdateDto);
        Task DeleteMenuAsync(int menuId);

        //Task AssignMenuToRoleAsync(int roleId, int menuId, bool canView, bool canAdd, bool canEdit, bool canDelete);
        Task AssignMenuToRoleAsync(RoleMenuDto roleMenuDto);
        Task UnassignMenuFromRoleAsync(int roleId, int menuId);
    }
}
