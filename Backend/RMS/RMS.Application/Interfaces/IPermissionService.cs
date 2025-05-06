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
        Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
        Task<PermissionDto> GetPermissionByIdAsync(int permissionId);
        Task<int> CreatePermissionAsync(PermissionCreateDto permissionCreateDto);
        Task UpdatePermissionAsync(PermissionUpdateDto permissionUpdateDto);
        Task DeletePermissionAsync(int permissionId);
    }
}
