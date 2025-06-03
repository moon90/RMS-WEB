using AutoMapper;
using FluentValidation;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs.RoleDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleDTOs.OutputDTOs;
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

        public RoleService(
            IRoleRepository roleRepository,
            IRolePermissionRepository rolePermissionRepository,
            IMapper mapper,
            IValidator<RoleCreateDto> roleCreateValidator,
            IValidator<RoleUpdateDto> roleUpdateValidator)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _mapper = mapper;
            _roleCreateValidator = roleCreateValidator;
            _roleUpdateValidator = roleUpdateValidator;
        }

        // Get role by ID
        public async Task<ResponseDto<RoleDto>> GetRoleByIdAsync(int roleId)
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

        // Get all roles
        public async Task<ResponseDto<IEnumerable<RoleDto>>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);

            if (!roleDtos.Any())
            {
                return new ResponseDto<IEnumerable<RoleDto>>
                {
                    IsSuccess = false,
                    Message = "No roles found.",
                    Code = "404",
                    Data = roleDtos
                };
            }

            return new ResponseDto<IEnumerable<RoleDto>>
            {
                IsSuccess = true,
                Message = "Roles retrieved successfully.",
                Code = "200",
                Data = roleDtos
            };
        }


        public async Task<PagedResult<RoleDto>> GetAllRolesAsync(int pageNumber, int pageSize)
        {
            var (roles, totalCount) = await _roleRepository.GetAllRolesAsync(pageNumber, pageSize);
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);

            // Wrap in PagedResult
            var pagedResult = new PagedResult<RoleDto>(roleDtos, pageNumber, pageSize, totalCount);
            return pagedResult;
        }

        public async Task<ResponseDto<RoleDto>> CreateRoleAsync(RoleCreateDto roleCreateDto)
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


        // Update an existing role
        public async Task<ResponseDto<string>> UpdateRoleAsync(RoleUpdateDto roleUpdateDto)
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


        // Delete a role
        public async Task<ResponseDto<string>> DeleteRoleAsync(int roleId)
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


        // Assign a permission to a role
        public async Task<ResponseDto<string>> AssignPermissionToRoleAsync(RolePermissionDto rolePermissionDto)
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


        // Unassign a permission from a role
        public async Task<ResponseDto<string>> UnassignPermissionFromRoleAsync(int roleId, int permissionId)
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


        public async Task<ResponseDto<List<int>>> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
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

            var assignedPermissions = new List<int>();

            foreach (var permissionId in permissionIds)
            {
                var isAssigned = await _rolePermissionRepository.IsPermissionAssignedToRoleAsync(roleId, permissionId);
                if (!isAssigned)
                {
                    await _rolePermissionRepository.AssignPermissionToRoleAsync(roleId, permissionId);
                    assignedPermissions.Add(permissionId);
                }
            }

            return new ResponseDto<List<int>>
            {
                IsSuccess = true,
                Message = "Permissions assigned successfully.",
                Code = "200",
                Data = assignedPermissions
            };
        }


        public async Task<ResponseDto<List<int>>> UnassignPermissionsFromRoleAsync(int roleId, List<int> permissionIds)
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

    }
}
