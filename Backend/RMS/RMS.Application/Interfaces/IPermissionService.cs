using RMS.Domain.Dtos;
using RMS.Domain.Dtos.PermissionDTOs.InputDTOs;
using RMS.Domain.Dtos.PermissionDTOs.OutputDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<ResponseDto<IEnumerable<PermissionDto>>> GetAllPermissionsAsync();
        Task<ResponseDto<PermissionDto>> GetPermissionByIdAsync(int permissionId);
        Task<ResponseDto<int>> CreatePermissionAsync(PermissionCreateDto permissionCreateDto);
        Task<ResponseDto<bool>> UpdatePermissionAsync(PermissionUpdateDto permissionUpdateDto);
        Task<ResponseDto<bool>> DeletePermissionAsync(int permissionId);
    }
}
