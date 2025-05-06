using AutoMapper;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos.PermissionDTOs.InputDTOs;
using RMS.Domain.Dtos.PermissionDTOs.OutputDTOs;
using RMS.Domain.Entities;
using RMS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
        {
            var permissions = await _permissionRepository.GetAllPermissionsAsync();
            return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }

        public async Task<PermissionDto> GetPermissionByIdAsync(int permissionId)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);
            if (permission == null)
                throw new ArgumentException("Permission not found.");

            return _mapper.Map<PermissionDto>(permission);
        }

        public async Task<int> CreatePermissionAsync(PermissionCreateDto permissionCreateDto)
        {
            var permission = _mapper.Map<Permission>(permissionCreateDto);
            await _permissionRepository.AddPermissionAsync(permission);
            return permission.Id;
        }

        public async Task UpdatePermissionAsync(PermissionUpdateDto permissionUpdateDto)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionUpdateDto.Id);
            if (permission == null)
                throw new ArgumentException("Permission not found.");

            _mapper.Map(permissionUpdateDto, permission);
            await _permissionRepository.UpdatePermissionAsync(permission);
        }

        public async Task DeletePermissionAsync(int permissionId)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);
            if (permission == null)
                throw new ArgumentException("Permission not found.");

            await _permissionRepository.DeletePermissionAsync(permissionId);
        }
    }
}
