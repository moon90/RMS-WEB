using RMS.Application.DTOs;
using RMS.Application.DTOs.PermissionDTOs.InputDTOs;
using RMS.Application.DTOs.PermissionDTOs.OutputDTOs;
using RMS.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<PagedResult<PermissionDto>> GetAllPermissionsAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<PermissionDto>> GetPermissionByIdAsync(int permissionId);
        Task<ResponseDto<PermissionDto>> CreatePermissionAsync(PermissionCreateDto permissionCreateDto);
        Task<ResponseDto<PermissionDto>> UpdatePermissionAsync(PermissionUpdateDto permissionUpdateDto);
        Task<ResponseDto<string>> DeletePermissionAsync(int permissionId);
    }
}
