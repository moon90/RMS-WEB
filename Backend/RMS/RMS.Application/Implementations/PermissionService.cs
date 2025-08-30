using AutoMapper;
using FluentValidation;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.Dtos.PermissionDTOs.InputDTOs;
using RMS.Domain.Dtos.PermissionDTOs.OutputDTOs;
using RMS.Domain.Entities;
using RMS.Domain.Models.BaseModels;
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

        public async Task<PagedResult<PermissionDto>> GetAllPermissionsAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            try
            {
                var (permissions, totalCount) = await _permissionRepository.GetAllPermissionsAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status);

                var permissionDtos = _mapper.Map<List<PermissionDto>>(permissions);

                var pagedResult = new PagedResult<PermissionDto>(permissionDtos, pageNumber, pageSize, totalCount);
                return pagedResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllPermissionsAsync (paged): {ex.Message}");
                throw; // Re-throw or handle as appropriate for your application's error handling strategy
            }
        }

        public async Task<ResponseDto<PermissionDto>> GetPermissionByIdAsync(int permissionId)
        {
            try
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
            catch (Exception ex)
            {
                return new ResponseDto<PermissionDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the permission.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<PermissionDto>> CreatePermissionAsync(PermissionCreateDto permissionCreateDto)
        {
            try
            {
                var validationResult = await _permissionCreateValidator.ValidateAsync(permissionCreateDto);
                if (!validationResult.IsValid)
                {
                    return new ResponseDto<PermissionDto>
                    {
                        IsSuccess = false,
                        Message = "Validation failed.",
                        Code = "400",
                        Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    };
                }

                if (await _permissionRepository.GetPermissionByNameAsync(permissionCreateDto.PermissionName) != null)
                {
                    return new ResponseDto<PermissionDto>
                    {
                        IsSuccess = false,
                        Message = "Permission name already exists.",
                        Code = "409"
                    };
                }

                if (await _permissionRepository.GetPermissionByKeyAsync(permissionCreateDto.PermissionKey) != null)
                {
                    return new ResponseDto<PermissionDto>
                    {
                        IsSuccess = false,
                        Message = "Permission key already exists.",
                        Code = "409"
                    };
                }

                var permission = _mapper.Map<Permission>(permissionCreateDto);
                await _permissionRepository.AddPermissionAsync(permission);

                var permissionDto = _mapper.Map<PermissionDto>(permission);

                return new ResponseDto<PermissionDto>
                {
                    IsSuccess = true,
                    Message = "Permission created successfully.",
                    Code = "201",
                    Data = permissionDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<PermissionDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the permission.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<PermissionDto>> UpdatePermissionAsync(PermissionUpdateDto permissionUpdateDto)
        {
            try
            {
                var validationResult = await _permissionUpdateValidator.ValidateAsync(permissionUpdateDto);
                if (!validationResult.IsValid)
                {
                    return new ResponseDto<PermissionDto>
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
                    return new ResponseDto<PermissionDto>
                    {
                        IsSuccess = false,
                        Message = "Permission not found.",
                        Code = "404"
                    };
                }

                var existingPermissionByName = await _permissionRepository.GetPermissionByNameAsync(permissionUpdateDto.PermissionName);
                if (existingPermissionByName != null && existingPermissionByName.Id != permissionUpdateDto.Id)
                {
                    return new ResponseDto<PermissionDto>
                    {
                        IsSuccess = false,
                        Message = "Permission name already exists.",
                        Code = "409"
                    };
                }

                var existingPermissionByKey = await _permissionRepository.GetPermissionByKeyAsync(permissionUpdateDto.PermissionKey);
                if (existingPermissionByKey != null && existingPermissionByKey.Id != permissionUpdateDto.Id)
                {
                    return new ResponseDto<PermissionDto>
                    {
                        IsSuccess = false,
                        Message = "Permission key already exists.",
                        Code = "409"
                    };
                }

                _mapper.Map(permissionUpdateDto, permission);
                await _permissionRepository.UpdatePermissionAsync(permission);

                var updatedPermissionDto = _mapper.Map<PermissionDto>(permission);

                return new ResponseDto<PermissionDto>
                {
                    IsSuccess = true,
                    Message = "Permission updated successfully.",
                    Code = "200",
                    Data = updatedPermissionDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<PermissionDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the permission.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<string>> DeletePermissionAsync(int permissionId)
        {
            try
            {
                var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);
                if (permission == null)
                {
                    return new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Permission not found.",
                        Code = "404"
                    };
                }

                await _permissionRepository.DeletePermissionAsync(permissionId);

                return new ResponseDto<string>
                {
                    IsSuccess = true,
                    Message = "Permission deleted successfully.",
                    Code = "200",
                    Data = $"PermissionId:{permissionId}"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the permission.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

    }
}
