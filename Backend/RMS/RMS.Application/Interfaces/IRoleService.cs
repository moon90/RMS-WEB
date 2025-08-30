using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs.RoleDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleDTOs.OutputDTOs;
using RMS.Domain.DTOs.RolePermissionDTOs.OutputDTOs;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.DTOs.RoleMenuDTOs.OutputDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseDto<RoleDto>> GetRoleByIdAsync(int roleId);
        Task<PagedResult<RoleDto>> GetAllRolesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection);

        Task<ResponseDto<RoleDto>> CreateRoleAsync(RoleCreateDto roleCreateDto);
        Task<ResponseDto<string>> UpdateRoleAsync(RoleUpdateDto roleUpdateDto);
        Task<ResponseDto<string>> DeleteRoleAsync(int roleId);

        Task<ResponseDto<string>> AssignPermissionToRoleAsync(RolePermissionDto rolePermissionDto);
        Task<ResponseDto<string>> UnassignPermissionFromRoleAsync(int roleId, int permissionId);

        Task<ResponseDto<List<int>>> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
        Task<ResponseDto<List<int>>> UnassignPermissionsFromRoleAsync(int roleId, List<int> permissionIds);
        Task<ResponseDto<IEnumerable<RolePermissionDto>>> GetRolePermissionsByRoleIdAsync(int roleId);
        Task<ResponseDto<IEnumerable<RoleMenuDto>>> GetRoleMenusByRoleIdAsync(int roleId);
    }
}
