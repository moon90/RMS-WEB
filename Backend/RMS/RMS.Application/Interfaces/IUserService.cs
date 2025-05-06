using RMS.Application.DTOs;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Domain.DTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<PagedResult<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize);
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<int> CreateUserAsync(UserCreateDto userCreateDto);
        Task UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task DeleteUserAsync(int userId);
        Task AssignRoleToUserAsync(int userId, int roleId);
        Task UnassignRoleFromUserAsync(int userId, int roleId);
        Task<UserDto> GetUserByRefreshTokenAsync(string refreshToken);
        Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime? refreshTokenExpiry);
        Task<UserDto> AuthenticateAsync(string username, string password);
        Task<List<string>> GetRolePermissionsAsync(int userId);
        Task<List<MenuPermissionDto>> GetMenuPermissionsAsync(int userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task SetPasswordResetTokenAsync(int userId, string resetToken, DateTime expiryTime);
        Task UpdateUserPasswordAsync(int userId, string passwordHash, string passwordSalt);
        Task AssignRolesToUserAsync(int userId, List<int> roleIds, string performedBy);
        Task UnassignRolesFromUserAsync(int userId, List<int> roleIds, string performedBy);
    }
}
