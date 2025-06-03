using AutoMapper;
using FluentValidation;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.Dtos.PermissionDTOs.InputDTOs;
using RMS.Domain.Dtos.PermissionDTOs.OutputDTOs;
using RMS.Domain.Entities;
using RMS.Infrastructure.Interfaces;

namespace RMS.Application.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<PermissionCreateDto> _permissionCreateValidator;
        private readonly IValidator<PermissionUpdateDto> _permissionUpdateValidator;

        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper, IValidator<PermissionCreateDto> permissionCreateValidator, IValidator<PermissionUpdateDto> permissionUpdateValidator)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _permissionCreateValidator = permissionCreateValidator;
            _permissionUpdateValidator = permissionUpdateValidator;
        }

        public async Task<ResponseDto<IEnumerable<PermissionDto>>> GetAllPermissionsAsync()
        {
            var permissions = await _permissionRepository.GetAllPermissionsAsync();
            var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);

            return new ResponseDto<IEnumerable<PermissionDto>>
            {
                IsSuccess = true,
                Message = "Permissions retrieved successfully.",
                Code = "200",
                Data = permissionDtos
            };
        }

        public async Task<ResponseDto<PermissionDto>> GetPermissionByIdAsync(int permissionId)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);

            if (permission == null)
            {
                return new ResponseDto<PermissionDto>
                {
                    IsSuccess = false,
                    Message = "Permission not found.",
                    Code = "404"
                };
            }

            var permissionDto = _mapper.Map<PermissionDto>(permission);

            return new ResponseDto<PermissionDto>
            {
                IsSuccess = true,
                Message = "Permission retrieved successfully.",
                Code = "200",
                Data = permissionDto
            };
        }

        public async Task<ResponseDto<int>> CreatePermissionAsync(PermissionCreateDto permissionCreateDto)
        {
            var validationResult = await _permissionCreateValidator.ValidateAsync(permissionCreateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<int>
                {
                    IsSuccess = false,
                    Message = "Validation failed.",
                    Code = "400",
                    Data = 0,
                    Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                };
            }

            var permission = _mapper.Map<Permission>(permissionCreateDto);
            await _permissionRepository.AddPermissionAsync(permission);

            return new ResponseDto<int>
            {
                IsSuccess = true,
                Message = "Permission created successfully.",
                Code = "201",
                Data = permission.Id
            };
        }

        public async Task<ResponseDto<bool>> UpdatePermissionAsync(PermissionUpdateDto permissionUpdateDto)
        {
            var validationResult = await _permissionUpdateValidator.ValidateAsync(permissionUpdateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<bool>
                {
                    IsSuccess = false,
                    Message = "Validation failed.",
                    Code = "400",
                    Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                };
            }

            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionUpdateDto.Id);
            if (permission == null)
            {
                return new ResponseDto<bool>
                {
                    IsSuccess = false,
                    Message = "Permission not found.",
                    Code = "404",
                    Data = false
                };
            }

            _mapper.Map(permissionUpdateDto, permission);
            await _permissionRepository.UpdatePermissionAsync(permission);

            return new ResponseDto<bool>
            {
                IsSuccess = true,
                Message = "Permission updated successfully.",
                Code = "200",
                Data = true
            };
        }

        public async Task<ResponseDto<bool>> DeletePermissionAsync(int permissionId)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);
            if (permission == null)
            {
                return new ResponseDto<bool>
                {
                    IsSuccess = false,
                    Message = "Permission not found.",
                    Code = "404",
                    Data = false
                };
            }

            await _permissionRepository.DeletePermissionAsync(permissionId);

            return new ResponseDto<bool>
            {
                IsSuccess = true,
                Message = "Permission deleted successfully.",
                Code = "200",
                Data = true
            };
        }

    }
}
