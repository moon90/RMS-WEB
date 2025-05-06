using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Domain.DTOs.RoleDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleDTOs.OutputDTOs;
using RMS.Domain.DTOs.RolePermissionDTOs.OutputDTOs;
using RMS.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto> GetRoleByIdAsync(int roleId);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<PagedResult<RoleDto>> GetAllRolesAsync(int pageNumber, int pageSize);
        Task<int> CreateRoleAsync(RoleCreateDto roleCreateDto);
        Task UpdateRoleAsync(RoleUpdateDto roleUpdateDto);
        Task DeleteRoleAsync(int roleId);
        Task AssignPermissionToRoleAsync(RolePermissionDto  rolePermissionDto);
        Task UnassignPermissionFromRoleAsync(int roleId, int permissionId);
        Task AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
        Task UnassignPermissionsFromRoleAsync(int roleId, List<int> permissionIds);
    }
}
