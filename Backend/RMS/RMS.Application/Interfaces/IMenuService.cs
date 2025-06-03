using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Domain.Dtos;
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
        Task<ResponseDto<MenuDto>> GetMenuByIdAsync(int menuId);
        Task<ResponseDto<IEnumerable<MenuDto>>> GetAllMenusAsync();
        Task<PagedResult<MenuDto>> GetAllMenusAsync(int pageNumber, int pageSize);
        Task<ResponseDto<int>> CreateMenuAsync(MenuCreateDto menuCreateDto);
        Task<ResponseDto<object>> UpdateMenuAsync(MenuUpdateDto menuUpdateDto);
        Task<ResponseDto<object>> DeleteMenuAsync(int menuId);

        Task<ResponseDto<object>> AssignMenuToRoleAsync(RoleMenuDto roleMenuDto);
        Task<ResponseDto<object>> UnassignMenuFromRoleAsync(int roleId, int menuId);
    }
}
