using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs.MenuDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleMenuDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleMenuDTOs.OutputDTOs;
using RMS.Domain.Models.BaseModels;

namespace RMS.Application.Interfaces
{
    public interface IMenuService
    {
        Task<ResponseDto<MenuDto>> GetMenuByIdAsync(int menuId);
        Task<ResponseDto<IEnumerable<MenuDto>>> GetAllMenusAsync();
        Task<PagedResult<MenuDto>> GetAllMenusAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection);
        Task<ResponseDto<int>> CreateMenuAsync(MenuCreateDto menuCreateDto);
        Task<ResponseDto<object>> UpdateMenuAsync(MenuUpdateDto menuUpdateDto);
        Task<ResponseDto<object>> DeleteMenuAsync(int menuId);

        Task<ResponseDto<object>> AssignMenuToRoleAsync(RoleMenuDto roleMenuDto);
        Task<ResponseDto<object>> UnassignMenuFromRoleAsync(int roleId, int menuId);
        Task<ResponseDto<object>> AssignMenusToRoleAsync(RoleMenuBulkUpdateDto dto);
        Task<ResponseDto<object>> UnassignMenusFromRoleAsync(RoleMenuBulkUpdateDto dto);
        Task<ResponseDto<IEnumerable<RoleMenuDto>>> GetMenusByRoleIdAsync(int roleId);
        Task<ResponseDto<IEnumerable<UserMenuPermissionDto>>> GetUserMenuPermissionsAsync(int userId);
    }
}
