using AutoMapper;
using FluentValidation;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Application.Helpers;
using RMS.Application.Interfaces;
using RMS.Domain.DTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.Domain.Entities;
using RMS.Domain.Models.BaseModels;
using RMS.Infrastructure.Interfaces;
using RMS.Infrastructure.IRepositories;

namespace RMS.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IAuditLogService _auditLogService;

        private readonly IMapper _mapper;
        private readonly IValidator<UserCreateDto> _userCreateValidator;
        private readonly IValidator<UserUpdateDto> _userUpdateValidator;

        public UserService(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IMapper mapper,
            IValidator<UserCreateDto> userCreateValidator,
            IValidator<UserUpdateDto> userUpdateValidator,
            IRolePermissionRepository rolePermissionRepository,
            IRoleMenuRepository roleMenuRepository,
            IAuditLogService auditLogService)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _userCreateValidator = userCreateValidator;
            _userUpdateValidator = userUpdateValidator;
            _rolePermissionRepository = rolePermissionRepository;
            _roleMenuRepository = roleMenuRepository;
            _auditLogService = auditLogService;
        }

        public async Task<UserDto> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return null;

            //var userDto = new UserDto()
            //{
            //    UserID = user.Id,
            //    UserName = user.UserName,
            //    FullName = user.FullName,
            //    Email = user.Email,
            //    Roles = user.UserRoles.Select(ur => ur.Role != null ? ur.Role.RoleName : string.Empty).ToList(),
            //    Phone = user.Phone,
            //    Status = user.Status,
            //    CreatedDate = user.CreatedDate,
            //    CreatedBy = user.CreatedBy,
            //    ModifiedDate = user.ModifiedDate,
            //    ModifiedBy = user.ModifiedBy,

            //};

            //var configCheck = _mapper.ConfigurationProvider;
            //configCheck.AssertConfigurationIsValid();

            //Console.WriteLine($"Mapping type: {user.GetType().FullName} → {typeof(UserDto).FullName}");
            
            return _mapper.Map<UserDto>(user);

            //return userDto;
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            return PasswordHelper.VerifyPassword(password, storedHash, storedSalt);
        }

        public async Task<List<string>> GetRolePermissionsAsync(int userId)
        {
            var userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(userId);
            var permissions = new List<string>();

            foreach (var role in userRoles)
            {
                var rolePermissions = await _rolePermissionRepository.GetPermissionsForRoleAsync(role.RoleID);
                permissions.AddRange(rolePermissions.Select(p => p.PermissionName));
            }

            return permissions.Distinct().ToList();
        }

        public async Task<List<MenuPermissionDto>> GetMenuPermissionsAsync(int userId)
        {
            var userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(userId);
            var menuPermissions = new List<MenuPermissionDto>();

            foreach (var role in userRoles)
            {
                var roleMenus = await _roleMenuRepository.GetRoleMenusByRoleIdAsync(role.RoleID);
                menuPermissions.AddRange(roleMenus.Select(m => new MenuPermissionDto
                {
                    MenuID = m.Menu.Id,
                    MenuName = m.Menu.MenuName,
                    CanView = m.CanView,
                    CanAdd = m.CanAdd,
                    CanEdit = m.CanEdit,
                    CanDelete = m.CanDelete
                }));
            }

            return menuPermissions;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync(); // new repository method
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<PagedResult<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            var (users, totalCount) = await _userRepository.GetAllUsersAsync(pageNumber, pageSize);
            var userDtos = _mapper.Map<List<UserDto>>(users);

            // Wrap in PagedResult
            var pagedResult = new PagedResult<UserDto>(userDtos, pageNumber, pageSize, totalCount);
            return pagedResult;
        }

        // Get user by ID
        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            return _mapper.Map<UserDto>(user);
        }

        // Get user by username
        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                throw new ArgumentException("User not found.");

            return _mapper.Map<UserDto>(user);
        }

        // Create a new user
        public async Task<int> CreateUserAsync(UserCreateDto userCreateDto)
        {
            // Validate input
            var validationResult = await _userCreateValidator.ValidateAsync(userCreateDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Hash the password

            // Generate hashed passwords and salts
            var (passwordHash, passwordSalt) = PasswordHelper.CreatePasswordHash(userCreateDto.Password);

            // Map the DTO to the User entity
            var user = _mapper.Map<User>(userCreateDto);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Save the user
            await _userRepository.AddUserAsync(user);
            return user.Id;
        }

        // Update an existing user
        public async Task UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            // Validate input
            var validationResult = await _userUpdateValidator.ValidateAsync(userUpdateDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = await _userRepository.GetUserByIdAsync(userUpdateDto.UserID);
            if (user == null)
                throw new ArgumentException("User not found.");

            _mapper.Map(userUpdateDto, user);
            await _userRepository.UpdateUserAsync(user);
        }

        // Delete a user
        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            await _userRepository.DeleteUserAsync(userId);

            await _auditLogService.LogAsync("DeleteUser", "User", $"UserId:{userId}", user.ModifiedBy);
        }

        // Assign a role to a user
        public async Task AssignRoleToUserAsync(int userId, int roleId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            await _userRoleRepository.AssignRoleToUserAsync(userId, roleId);
        }

        // Unassign a role from a user
        public async Task UnassignRoleFromUserAsync(int userId, int roleId)
        {
            await _userRoleRepository.UnassignRoleFromUserAsync(userId, roleId);
        }

        public async Task<UserDto> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime? refreshTokenExpiry)
        {
            await _userRepository.UpdateRefreshTokenAsync(userId, refreshToken, refreshTokenExpiry);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task SetPasswordResetTokenAsync(int userId, string resetToken, DateTime expiryTime)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new ArgumentException("User not found.");

            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = expiryTime;

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task UpdateUserPasswordAsync(int userId, string passwordHash, string passwordSalt)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new ArgumentException("User not found.");

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null; // clear reset token after use
            user.PasswordResetTokenExpiry = null;

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task AssignRolesToUserAsync(int userId, List<int> roleIds, string performedBy)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            foreach (var roleId in roleIds)
            {
                bool isAssigned = await _userRoleRepository.IsRoleAssignedToUserAsync(userId, roleId);
                if (!isAssigned)
                {
                    await _userRoleRepository.AssignRoleToUserAsync(userId, roleId);
                    await _auditLogService.LogAsync(
                        action: "AssignRole",
                        entityType: "UserRole",
                        entityId: $"UserId:{userId}-RoleId:{roleId}",
                        performedBy: performedBy,
                        details: "Role assigned via bulk operation"
                    );
                }
            }
        }

        public async Task UnassignRolesFromUserAsync(int userId, List<int> roleIds, string performedBy)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            foreach (var roleId in roleIds)
            {
                bool isAssigned = await _userRoleRepository.IsRoleAssignedToUserAsync(userId, roleId);
                if (isAssigned)
                {
                    await _userRoleRepository.UnassignRoleFromUserAsync(userId, roleId);
                    await _auditLogService.LogAsync(
                        action: "UnassignRole",
                        entityType: "UserRole",
                        entityId: $"UserId:{userId}-RoleId:{roleId}",
                        performedBy: performedBy,
                        details: "Role unassigned via bulk operation"
                    );
                }
            }
        }


    }
}
