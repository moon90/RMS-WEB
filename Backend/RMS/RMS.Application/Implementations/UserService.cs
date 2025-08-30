using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Application.Helpers;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs;
using RMS.Domain.DTOs.RoleDTOs.OutputDTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.Domain.Entities;
using RMS.Domain.Models.BaseModels;
using RMS.Infrastructure.Interfaces;
using RMS.Infrastructure.IRepositories;
using System.Linq;

namespace RMS.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly IRoleRepository _roleRepository; // Added
        private readonly IMenuService _menuService; // Added

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
            IAuditLogService auditLogService,
            IRoleRepository roleRepository,
            IMenuService menuService) // Added
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _userCreateValidator = userCreateValidator;
            _userUpdateValidator = userUpdateValidator;
            _rolePermissionRepository = rolePermissionRepository;
            _roleMenuRepository = roleMenuRepository;
            _auditLogService = auditLogService;
            _roleRepository = roleRepository;
            _menuService = menuService;
        } // Assign the injected roleRepository

        public async Task<ResponseDto<UserDto>> AuthenticateAsync(string username, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user == null || !VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                {
                    return new ResponseDto<UserDto>
                    {
                        IsSuccess = false,
                        Message = "Invalid username or password.",
                        Code = "401"
                    };
                }

                var userDto = _mapper.Map<UserDto>(user);

                // Populate roles for the UserDto
                var userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(user.Id);
                if (userRoles != null && userRoles.Any())
                {
                    foreach (var userRole in userRoles)
                    {
                        var role = await _roleRepository.GetRoleByIdAsync(userRole.RoleID);
                        if (role != null)
                        {
                            userDto.Roles.Add(role.RoleName);
                        }
                    }
                }

                return new ResponseDto<UserDto>
                {
                    IsSuccess = true,
                    Message = "Authentication successful.",
                    Code = "200",
                    Data = userDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred during authentication.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            return PasswordHelper.VerifyPassword(password, storedHash, storedSalt);
        }

        public async Task<ResponseDto<List<string>>> GetRolePermissionsAsync(int userId)
        {
            try
            {
                var userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(userId);

                if (userRoles == null || !userRoles.Any())
                {
                    return new ResponseDto<List<string>>
                    {
                        IsSuccess = false,
                        Message = "User has no roles assigned.",
                        Code = "404"
                    };
                }

                var permissions = new List<string>();

                foreach (var role in userRoles)
                {
                    var rolePermissions = await _rolePermissionRepository.GetPermissionsForRoleAsync(role.RoleID);
                    permissions.AddRange(rolePermissions.Select(p => p.PermissionKey));
                }

                var distinctPermissions = permissions.Distinct().ToList();

                

                if (!distinctPermissions.Any())
                {
                    return new ResponseDto<List<string>>
                    {
                        IsSuccess = false,
                        Message = "No permissions found for assigned roles.",
                        Code = "403"
                    };
                }

                return new ResponseDto<List<string>>
                {
                    IsSuccess = true,
                    Message = "Permissions retrieved successfully.",
                    Code = "200",
                    Data = distinctPermissions
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<string>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving role permissions.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<IEnumerable<UserMenuPermissionDto>>> GetMenuPermissionsAsync(int userId)
        {
            return await _menuService.GetUserMenuPermissionsAsync(userId);
        }

        public async Task<ResponseDto<IEnumerable<UserRole>>> GetUserRolesByUserIdAsync(int userId)
        {
            try
            {
                var userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(userId);

                if (userRoles == null || !userRoles.Any())
                {
                    return new ResponseDto<IEnumerable<UserRole>>
                    {
                        IsSuccess = false,
                        Message = "No roles found for this user.",
                        Code = "404"
                    };
                }

                return new ResponseDto<IEnumerable<UserRole>>
                {
                    IsSuccess = true,
                    Message = "User roles retrieved successfully.",
                    Code = "200",
                    Data = userRoles
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<UserRole>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving user roles.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        public async Task<ResponseDto<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    return new ResponseDto<IEnumerable<UserDto>>
                    {
                        IsSuccess = false,
                        Message = "No users found.",
                        Code = "404"
                    };
                }

                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

                return new ResponseDto<IEnumerable<UserDto>>
                {
                    IsSuccess = true,
                    Message = "Users retrieved successfully.",
                    Code = "200",
                    Data = userDtos
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<UserDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving users.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<PagedResult<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status, string? role)
        {
            try
            {
                var (users, totalCount) = await _userRepository.GetAllUsersAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status, role);

                var userDtos = _mapper.Map<List<UserDto>>(users);

                var pagedResult = new PagedResult<UserDto>(userDtos, pageNumber, pageSize, totalCount);
                return pagedResult;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as appropriate for your application's error handling strategy
                // For now, returning an empty PagedResult on error.
                return new PagedResult<UserDto>(new List<UserDto>(), pageNumber, pageSize, 0);
            }
        }

        public async Task<ResponseDto<UserDto>> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new ResponseDto<UserDto>
                    {
                        IsSuccess = false,
                        Message = "User not found.",
                        Code = "404"
                    };
                }

                return new ResponseDto<UserDto>
                {
                    IsSuccess = true,
                    Message = "User retrieved successfully.",
                    Code = "200",
                    Data = _mapper.Map<UserDto>(user)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the user.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<UserDto>> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);

                if (user == null)
                {
                    return new ResponseDto<UserDto>
                    {
                        IsSuccess = false,
                        Message = "User not found.",
                        Code = "404"
                    };
                }

                var userDto = _mapper.Map<UserDto>(user);

                return new ResponseDto<UserDto>
                {
                    IsSuccess = true,
                    Message = "User retrieved successfully.",
                    Code = "200",
                    Data = userDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the user by username.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<UserDto>> CreateUserAsync(UserCreateDto userCreateDto)
        {
            try
            {
                var validationResult = await _userCreateValidator.ValidateAsync(userCreateDto);
                if (!validationResult.IsValid)
                {
                    return new ResponseDto<UserDto>
                    {
                        IsSuccess = false,
                        Message = "Validation failed.",
                        Code = "400",
                        Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    };
                }

                var (passwordHash, passwordSalt) = PasswordHelper.CreatePasswordHash(userCreateDto.Password);

                var user = _mapper.Map<User>(userCreateDto);

                if (string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    user.ProfilePictureUrl = "/images/profiles/default.png";
                }

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _userRepository.AddUserAsync(user);

                var defaultRole = await _roleRepository.GetRoleByNameAsync("User");
                if (defaultRole != null)
                {
                    await _userRoleRepository.AssignRoleToUserAsync(user.Id, defaultRole.Id, userCreateDto.UserName);
                }
                else
                {
                    return new ResponseDto<UserDto>
                    {
                        IsSuccess = false,
                        Message = "User created, but default 'User' role not found or assigned.",
                        Code = "404"
                    };
                }

                var userDto = _mapper.Map<UserDto>(user);

                // ADD AUDIT LOG HERE
                await _auditLogService.LogAsync(
                    action: "CreateUser",
                    entityType: "User",
                    entityId: $"UserId:{user.Id}",
                    performedBy: user.UserName, // Or the user who performed the action if available (e.g., admin)
                    details: $"New user '{user.UserName}' created."
                );
                // END AUDIT LOG

                return new ResponseDto<UserDto>
                {
                    IsSuccess = true,
                    Message = "User created successfully.",
                    Code = "201",
                    Data = userDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the user.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<UserDto>> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var validationResult = await _userUpdateValidator.ValidateAsync(userUpdateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "Validation failed.",
                    Code = "400",
                    Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                };
            }

            var user = await _userRepository.GetUserByIdAsync(userUpdateDto.UserID);
            if (user == null)
            {
                return new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Code = "404"
                };
            }

            _mapper.Map(userUpdateDto, user);

            // This block handles profile picture URL update, but the actual file deletion
            // is now handled in the UsersController.
            if (!string.IsNullOrEmpty(userUpdateDto.ProfilePictureUrl) && user.ProfilePictureUrl != userUpdateDto.ProfilePictureUrl)
            {
                // The old file deletion logic is moved to the controller
                // if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                // {
                //     var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePictureUrl.TrimStart('/'));
                //     if (File.Exists(oldFilePath))
                //     {
                //         File.Delete(oldFilePath);
                //     }
                // }

                user.ProfilePictureUrl = userUpdateDto.ProfilePictureUrl;
            }
            else if (string.IsNullOrEmpty(userUpdateDto.ProfilePictureUrl) && !string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                // If the DTO explicitly clears the URL, and there was an old one, clear it in the entity
                user.ProfilePictureUrl = null;
            }


            await _userRepository.UpdateUserAsync(user);

            var updatedUserDto = _mapper.Map<UserDto>(user);

            return new ResponseDto<UserDto>
            {
                IsSuccess = true,
                Message = "User updated successfully.",
                Code = "200",
                Data = updatedUserDto
            };
        }

        public async Task<ResponseDto<string>> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Code = "404"
                };
            }

            await _userRepository.DeleteUserAsync(userId);

            await _auditLogService.LogAsync(
                action: "DeleteUser",
                entityType: "User",
                entityId: $"UserId:{userId}",
                performedBy: user.UserName, 
                details: $"User with ID {userId} was deleted."
            );

            return new ResponseDto<string>
            {
                IsSuccess = true,
                Message = "User deleted successfully.",
                Code = "200",
                Data = $"UserId:{userId}"
            };
        }


        public async Task<ResponseDto<string>> AssignRoleToUserAsync(int userId, int roleId, string performedBy)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Code = "404"
                };
            }

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

            await _userRoleRepository.AssignRoleToUserAsync(userId, roleId, performedBy);

            return new ResponseDto<string>
            {
                IsSuccess = true,
                Message = "Role assigned to user successfully.",
                Code = "200",
                Data = $"UserId:{userId}, RoleId:{roleId}"
            };
        }

        public async Task<ResponseDto<string>> UnassignRoleFromUserAsync(int userId, int roleId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Code = "404"
                };
            }

            var isAssigned = await _userRoleRepository.IsRoleAssignedToUserAsync(userId, roleId);
            if (!isAssigned)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "Role is not assigned to the user.",
                    Code = "404"
                };
            }

            await _userRoleRepository.UnassignRoleFromUserAsync(userId, roleId);

            return new ResponseDto<string>
            {
                IsSuccess = true,
                Message = "Role unassigned from user successfully.",
                Code = "200",
                Data = $"UserId:{userId}, RoleId:{roleId}"
            };
        }


        public async Task<ResponseDto<UserDto>> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                return new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "User not found for the given refresh token.",
                    Code = "401"
                };
            }

            var userDto = _mapper.Map<UserDto>(user);

            return new ResponseDto<UserDto>
            {
                IsSuccess = true,
                Message = "User retrieved successfully.",
                Code = "200",
                Data = userDto
            };
        }

        public async Task<ResponseDto<string>> UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime? refreshTokenExpiry)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Code = "404"
                };
            }

            await _userRepository.UpdateRefreshTokenAsync(userId, refreshToken, refreshTokenExpiry);

            return new ResponseDto<string>
            {
                IsSuccess = true,
                Message = "Refresh token updated successfully.",
                Code = "200",
                Data = $"UserId:{userId}"
            };
        }

        public async Task<ResponseDto<UserDto>> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "User not found with the given email.",
                    Code = "404"
                };
            }

            var userDto = _mapper.Map<UserDto>(user);

            return new ResponseDto<UserDto>
            {
                IsSuccess = true,
                Message = "User retrieved successfully.",
                Code = "200",
                Data = userDto
            };
        }

        public async Task<ResponseDto<string>> SetPasswordResetTokenAsync(int userId, string resetToken, DateTime expiryTime)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Code = "404"
                };
            }

            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = expiryTime;

            await _userRepository.UpdateUserAsync(user);

            return new ResponseDto<string>
            {
                IsSuccess = true,
                Message = "Password reset token set successfully.",
                Code = "200",
                Data = $"UserId:{userId}"
            };
        }

        public async Task<ResponseDto<string>> UpdateUserPasswordAsync(int userId, string passwordHash, string passwordSalt)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Code = "404"
                };
            }

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _userRepository.UpdateUserAsync(user);

            return new ResponseDto<string>
            {
                IsSuccess = true,
                Message = "Password updated successfully.",
                Code = "200",
                Data = $"UserId:{userId}"
            };
        }

        public async Task<ResponseDto<List<int>>> AssignRolesToUserAsync(int userId, List<int> roleIds, string performedBy)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto<List<int>>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Code = "404"
                };
            }

            var assignedRoles = new List<int>();
            var notFoundRoles = new List<int>();

            foreach (var roleId in roleIds)
            {
                var role = await _roleRepository.GetRoleByIdAsync(roleId);
                if (role == null)
                {
                    notFoundRoles.Add(roleId);
                    continue;
                }

                bool isAssigned = await _userRoleRepository.IsRoleAssignedToUserAsync(userId, roleId);
                if (!isAssigned)
                {
                    await _userRoleRepository.AssignRoleToUserAsync(userId, roleId, performedBy);
                    assignedRoles.Add(roleId);

                    await _auditLogService.LogAsync(
                        action: "AssignRole",
                        entityType: "UserRole",
                        entityId: $"UserId:{userId}-RoleId:{roleId}",
                        performedBy: performedBy,
                        details: "Role assigned via bulk operation"
                    );
                }
            }

            if (notFoundRoles.Any())
            {
                return new ResponseDto<List<int>>
                {
                    IsSuccess = false,
                    Message = $"Some roles were not found: {string.Join(", ", notFoundRoles)}",
                    Code = "404"
                };
            }

            if (assignedRoles.Count == 0)
            {
                return new ResponseDto<List<int>>
                {
                    IsSuccess = false,
                    Message = "No new roles were assigned. All roles already assigned.",
                    Code = "409",
                    Data = new List<int>()
                };
            }

            return new ResponseDto<List<int>>
            {
                IsSuccess = true,
                Message = "Roles assigned successfully.",
                Code = "200",
                Data = assignedRoles
            };
        }

        public async Task<ResponseDto<List<int>>> UnassignRolesFromUserAsync(int userId, List<int> roleIds, string performedBy)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto<List<int>>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Code = "404"
                };
            }

            var unassignedRoles = new List<int>();

            foreach (var roleId in roleIds)
            {
                bool isAssigned = await _userRoleRepository.IsRoleAssignedToUserAsync(userId, roleId);
                if (isAssigned)
                {
                    await _userRoleRepository.UnassignRoleFromUserAsync(userId, roleId);
                    unassignedRoles.Add(roleId);

                    await _auditLogService.LogAsync(
                        action: "UnassignRole",
                        entityType: "UserRole",
                        entityId: $"UserId:{userId}-RoleId:{roleId}",
                        performedBy: performedBy,
                        details: "Role unassigned via bulk operation"
                    );
                }
            }

            if (unassignedRoles.Count == 0)
            {
                return new ResponseDto<List<int>>
                {
                    IsSuccess = false,
                    Message = "No roles were unassigned. Roles may not have been assigned.",
                    Code = "409"
                };
            }

            return new ResponseDto<List<int>>
            {
                IsSuccess = true,
                Message = "Roles unassigned successfully.",
                Code = "200",
                Data = unassignedRoles
            };
        }

        public async Task<ResponseDto<string>> UploadProfilePictureAsync(int userId, string imageUrl) // Modified signature
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Code = "404"
                };
            }

            // The old profile picture should be deleted by the controller before calling this method.
            // This method only updates the URL in the database.
            user.ProfilePictureUrl = imageUrl;
            await _userRepository.UpdateUserAsync(user);

            return new ResponseDto<string>
            {
                IsSuccess = true,
                Message = "Profile picture uploaded successfully.",
                Code = "200",
                Data = imageUrl // Return the new URL
            };
        }

    }
}