using AutoMapper;
using FluentValidation;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs.RoleDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleDTOs.OutputDTOs;
using RMS.Domain.DTOs.RoleMenuDTOs.OutputDTOs;
using RMS.Domain.DTOs.RolePermissionDTOs.OutputDTOs;
using RMS.Domain.Entities;
using RMS.Domain.Models.BaseModels;
using RMS.Infrastructure.Interfaces;

namespace RMS.Application.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RoleCreateDto> _roleCreateValidator;
        private readonly IValidator<RoleUpdateDto> _roleUpdateValidator;
        private readonly IRoleMenuRepository _roleMenuRepository; // Added

        public RoleService(
            IRoleRepository roleRepository,
            IRolePermissionRepository rolePermissionRepository,
            IRoleMenuRepository roleMenuRepository, // Added
            IMapper mapper,
            IValidator<RoleCreateDto> roleCreateValidator,
            IValidator<RoleUpdateDto> roleUpdateValidator)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _roleMenuRepository = roleMenuRepository; // Added
            _mapper = mapper;
            _roleCreateValidator = roleCreateValidator;
            _roleUpdateValidator = roleUpdateValidator;
        }

        // Get role by ID
        public async Task<ResponseDto<RoleDto>> GetRoleByIdAsync(int roleId)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(roleId);
                if (role == null)
                {
                    return new ResponseDto<RoleDto>
                    {
                        IsSuccess = false,
                        Message = "Role not found.",
                        Code = "404"
                    };
                }

                var roleDto = _mapper.Map<RoleDto>(role);

                return new ResponseDto<RoleDto>
                {
                    IsSuccess = true,
                    Message = "Role retrieved successfully.",
                    Code = "200",
                    Data = roleDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<RoleDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<PagedResult<RoleDto>> GetAllRolesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection)
        {
            try
            {
                var (roles, totalCount) = await _roleRepository.GetAllRolesAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection);
                var roleDtos = _mapper.Map<List<RoleDto>>(roles);

                var pagedResult = new PagedResult<RoleDto>(roleDtos, pageNumber, pageSize, totalCount);
                return pagedResult;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetAllRolesAsync (paged): {ex.Message}");
                throw; // Re-throw or handle as appropriate for your application's error handling strategy
            }
        }

        public async Task<ResponseDto<RoleDto>> CreateRoleAsync(RoleCreateDto roleCreateDto)
        {
            try
            {
                var validationResult = await _roleCreateValidator.ValidateAsync(roleCreateDto);
                if (!validationResult.IsValid)
                {
                    return new ResponseDto<RoleDto>
                    {
                        IsSuccess = false,
                        Message = "Validation failed.",
                        Code = "400",
                        Details = validationResult.Errors.Select(e => new
                        {
                            e.PropertyName,
                            e.ErrorMessage
                        })
                    };
                }

                // Check if role name already exists
                if (await _roleRepository.GetRoleByNameAsync(roleCreateDto.RoleName) != null)
                {
                    return new ResponseDto<RoleDto>
                    {
                        IsSuccess = false,
                        Message = "Role name already exists.",
                        Code = "409"
                    };
                }

                var role = _mapper.Map<Role>(roleCreateDto);
                await _roleRepository.AddRoleAsync(role);

                var roleDto = _mapper.Map<RoleDto>(role);

                return new ResponseDto<RoleDto>
                {
                    IsSuccess = true,
                    Message = "Role created successfully.",
                    Code = "201",
                    Data = roleDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<RoleDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        // Update an existing role
        public async Task<ResponseDto<string>> UpdateRoleAsync(RoleUpdateDto roleUpdateDto)
        {
            try
            {
                var validationResult = await _roleUpdateValidator.ValidateAsync(roleUpdateDto);
                if (!validationResult.IsValid)
                {
                    return new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Validation failed.",
                        Code = "400",
                        Details = validationResult.Errors.Select(e => new
                        {
                            e.PropertyName,
                            e.ErrorMessage
                        })
                    };
                }

                var role = await _roleRepository.GetRoleByIdAsync(roleUpdateDto.RoleID);
                if (role == null)
                {
                    return new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Role not found.",
                        Code = "404"
                    };
                }

                // Check if role name already exists for another role
                var existingRole = await _roleRepository.GetRoleByNameAsync(roleUpdateDto.RoleName);
                if (existingRole != null && existingRole.Id != roleUpdateDto.RoleID)
                {
                    return new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Role name already exists.",
                        Code = "409"
                    };
                }

                _mapper.Map(roleUpdateDto, role);
                await _roleRepository.UpdateRoleAsync(role);

                return new ResponseDto<string>
                {
                    IsSuccess = true,
                    Message = "Role updated successfully.",
                    Code = "200",
                    Data = $"RoleId:{roleUpdateDto.RoleID}"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        // Delete a role
        public async Task<ResponseDto<string>> DeleteRoleAsync(int roleId)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(roleId);
                if (role == null)
                {
                    return new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Role not found.",
                        Code = "404"
                    };
                }

                await _roleRepository.DeleteRoleAsync(roleId);

                return new ResponseDto<string>
                {
                    IsSuccess = true,
                    Message = "Role deleted successfully.",
                    Code = "200",
                    Data = $"RoleId:{roleId}"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        // Assign a permission to a role
        public async Task<ResponseDto<string>> AssignPermissionToRoleAsync(RolePermissionDto rolePermissionDto)
        {
            try
            {
                var isAssigned = await _rolePermissionRepository.IsPermissionAssignedToRoleAsync(
                    rolePermissionDto.RoleID, rolePermissionDto.PermissionID);

                if (isAssigned)
                {
                    return new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Permission is already assigned to the role.",
                        Code = "409"
                    };
                }

                await _rolePermissionRepository.AssignPermissionToRoleAsync(
                    rolePermissionDto.RoleID,
                    rolePermissionDto.PermissionID,
                    rolePermissionDto.SortingOrder);

                return new ResponseDto<string>
                {
                    IsSuccess = true,
                    Message = "Permission assigned successfully.",
                    Code = "200",
                    Data = $"RoleId:{rolePermissionDto.RoleID}-PermissionId:{rolePermissionDto.PermissionID}"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning the permission.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        // Unassign a permission from a role
        public async Task<ResponseDto<string>> UnassignPermissionFromRoleAsync(int roleId, int permissionId)
        {
            try
            {
                var isAssigned = await _rolePermissionRepository.IsPermissionAssignedToRoleAsync(roleId, permissionId);
                if (!isAssigned)
                {
                    return new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Permission is not assigned to the role.",
                        Code = "403"
                    };
                }

                await _rolePermissionRepository.UnassignPermissionFromRoleAsync(roleId, permissionId);

                return new ResponseDto<string>
                {
                    IsSuccess = true,
                    Message = "Permission unassigned successfully.",
                    Code = "200",
                    Data = $"RoleId:{roleId}-PermissionId:{permissionId}"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while unassigning the permission.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        public async Task<ResponseDto<List<int>>> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(roleId);
                if (role == null)
                {
                    return new ResponseDto<List<int>>
                    {
                        IsSuccess = false,
                        Message = "Role not found.",
                        Code = "404"
                    };
                }

                await _rolePermissionRepository.AssignPermissionsToRoleAsync(roleId, permissionIds);

                return new ResponseDto<List<int>>
                {
                    IsSuccess = true,
                    Message = "Permissions assigned successfully.",
                    Code = "200",
                    Data = permissionIds
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<int>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning permissions.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        public async Task<ResponseDto<IEnumerable<RolePermissionDto>>> GetRolePermissionsByRoleIdAsync(int roleId)
        {
            try
            {
                var rolePermissions = await _rolePermissionRepository.GetRolePermissionsByRoleIdAsync(roleId);

                if (rolePermissions == null || !rolePermissions.Any())
                {
                    return new ResponseDto<IEnumerable<RolePermissionDto>>
                    {
                        IsSuccess = false,
                        Message = "No permissions found for this role.",
                        Code = "204"
                    };
                }

                var rolePermissionDtos = _mapper.Map<IEnumerable<RolePermissionDto>>(rolePermissions);

                return new ResponseDto<IEnumerable<RolePermissionDto>>
                {
                    IsSuccess = true,
                    Message = "Permissions retrieved successfully for the role.",
                    Code = "200",
                    Data = rolePermissionDtos
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<RolePermissionDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving permissions for the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<List<int>>> UnassignPermissionsFromRoleAsync(int roleId, List<int> permissionIds)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(roleId);
                if (role == null)
                {
                    return new ResponseDto<List<int>>
                    {
                        IsSuccess = false,
                        Message = "Role not found.",
                        Code = "404"
                    };
                }

                var unassignedPermissions = new List<int>();

                foreach (var permissionId in permissionIds)
                {
                    var isAssigned = await _rolePermissionRepository.IsPermissionAssignedToRoleAsync(roleId, permissionId);
                    if (isAssigned)
                    {
                        await _rolePermissionRepository.UnassignPermissionFromRoleAsync(roleId, permissionId);
                        unassignedPermissions.Add(permissionId);
                    }
                }

                return new ResponseDto<List<int>>
                {
                    IsSuccess = true,
                    Message = "Permissions unassigned successfully.",
                    Code = "200",
                    Data = unassignedPermissions
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<int>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while unassigning permissions.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<IEnumerable<RoleMenuDto>>> GetRoleMenusByRoleIdAsync(int roleId)
        {
            try
            {
                var roleMenus = await _roleMenuRepository.GetRoleMenusByRoleIdAsync(roleId);

                if (roleMenus == null || !roleMenus.Any())
                {
                    return new ResponseDto<IEnumerable<RoleMenuDto>>
                    {
                        IsSuccess = false,
                        Message = "No menus found for this role.",
                        Code = "204"
                    };
                }

                var roleMenuDtos = _mapper.Map<IEnumerable<RoleMenuDto>>(roleMenus);

                return new ResponseDto<IEnumerable<RoleMenuDto>>
                {
                    IsSuccess = true,
                    Message = "Menus retrieved successfully for the role.",
                    Code = "200",
                    Data = roleMenuDtos
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<RoleMenuDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving menus for the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }
    }
}
