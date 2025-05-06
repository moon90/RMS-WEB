using AutoMapper;
using FluentValidation;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.Interfaces;
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
        public async Task<RoleDto> GetRoleByIdAsync(int roleId)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            if (role == null)
                throw new ArgumentException("Role not found.");

            return _mapper.Map<RoleDto>(role);
        }

        // Get all roles
        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<PagedResult<RoleDto>> GetAllRolesAsync(int pageNumber, int pageSize)
        {
            var (roles, totalCount) = await _roleRepository.GetAllRolesAsync(pageNumber, pageSize);
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);

            // Wrap in PagedResult
            var pagedResult = new PagedResult<RoleDto>(roleDtos, pageNumber, pageSize, totalCount);
            return pagedResult;
        }

        // Create a new role
        public async Task<int> CreateRoleAsync(RoleCreateDto roleCreateDto)
        {
            // Validate input
            var validationResult = await _roleCreateValidator.ValidateAsync(roleCreateDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var role = _mapper.Map<Role>(roleCreateDto);
            await _roleRepository.AddRoleAsync(role);
            return role.Id;
        }

        // Update an existing role
        public async Task UpdateRoleAsync(RoleUpdateDto roleUpdateDto)
        {
            // Validate input
            var validationResult = await _roleUpdateValidator.ValidateAsync(roleUpdateDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var role = await _roleRepository.GetRoleByIdAsync(roleUpdateDto.RoleID);
            if (role == null)
                throw new ArgumentException("Role not found.");

            _mapper.Map(roleUpdateDto, role);
            await _roleRepository.UpdateRoleAsync(role);
        }

        // Delete a role
        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            if (role == null)
                throw new ArgumentException("Role not found.");

            await _roleRepository.DeleteRoleAsync(roleId);
        }

        // Assign a permission to a role
        public async Task AssignPermissionToRoleAsync(RolePermissionDto rolePermissionDto)
        {
            // Check if the permission is already assigned to the role
            var isAssigned = await _rolePermissionRepository.IsPermissionAssignedToRoleAsync(rolePermissionDto.RoleID, rolePermissionDto.PermissionID);
            if (isAssigned)
                throw new ArgumentException("Permission is already assigned to the role.");

            // Assign the permission to the role
            await _rolePermissionRepository.AssignPermissionToRoleAsync(rolePermissionDto.RoleID, rolePermissionDto.PermissionID, rolePermissionDto.SortingOrder);
        }

        // Unassign a permission from a role
        public async Task UnassignPermissionFromRoleAsync(int roleId, int permissionId)
        {
            // Check if the permission is assigned to the role
            var isAssigned = await _rolePermissionRepository.IsPermissionAssignedToRoleAsync(roleId, permissionId);
            if (!isAssigned)
                throw new ArgumentException("Permission is not assigned to the role.");

            // Unassign the permission from the role
            await _rolePermissionRepository.UnassignPermissionFromRoleAsync(roleId, permissionId);
        }

        public async Task AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            if (role == null)
                throw new ArgumentException("Role not found.");

            foreach (var permissionId in permissionIds)
            {
                bool isAssigned = await _rolePermissionRepository.IsPermissionAssignedToRoleAsync(roleId, permissionId);
                if (!isAssigned)
                {
                    await _rolePermissionRepository.AssignPermissionToRoleAsync(roleId, permissionId);
                }
            }
        }

        public async Task UnassignPermissionsFromRoleAsync(int roleId, List<int> permissionIds)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            if (role == null)
                throw new ArgumentException("Role not found.");

            foreach (var permissionId in permissionIds)
            {
                bool isAssigned = await _rolePermissionRepository.IsPermissionAssignedToRoleAsync(roleId, permissionId);
                if (isAssigned)
                {
                    await _rolePermissionRepository.UnassignPermissionFromRoleAsync(roleId, permissionId);
                }
            }
        }

    }
}
