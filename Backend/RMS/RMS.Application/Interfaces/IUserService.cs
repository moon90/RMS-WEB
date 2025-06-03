using Microsoft.AspNetCore.Http;
using RMS.Application.DTOs;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Domain.Dtos;
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
        // Authentication
        Task<ResponseDto<UserDto>> AuthenticateAsync(string username, string password);

        // Get user details
        Task<ResponseDto<UserDto>> GetUserByIdAsync(int userId);
        Task<ResponseDto<UserDto>> GetUserByUsernameAsync(string username);
        Task<ResponseDto<UserDto>> GetUserByEmailAsync(string email);
        Task<ResponseDto<UserDto>> GetUserByRefreshTokenAsync(string refreshToken);

        // User CRUD
        Task<ResponseDto<UserDto>> CreateUserAsync(UserCreateDto userCreateDto);
        Task<ResponseDto<UserDto>> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<ResponseDto<string>> DeleteUserAsync(int userId);

        // User listing
        Task<ResponseDto<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<PagedResult<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize); // can keep raw since it's a custom wrapper

        // Role & Permission
        Task<ResponseDto<List<string>>> GetRolePermissionsAsync(int userId);
        Task<ResponseDto<List<MenuPermissionDto>>> GetMenuPermissionsAsync(int userId);

        // Role assignment
        Task<ResponseDto<string>> AssignRoleToUserAsync(int userId, int roleId);
        Task<ResponseDto<string>> UnassignRoleFromUserAsync(int userId, int roleId);
        Task<ResponseDto<List<int>>> AssignRolesToUserAsync(int userId, List<int> roleIds, string performedBy);
        Task<ResponseDto<List<int>>> UnassignRolesFromUserAsync(int userId, List<int> roleIds, string performedBy);

        // Password and tokens
        Task<ResponseDto<string>> SetPasswordResetTokenAsync(int userId, string resetToken, DateTime expiryTime);
        Task<ResponseDto<string>> UpdateUserPasswordAsync(int userId, string passwordHash, string passwordSalt);
        Task<ResponseDto<string>> UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime? refreshTokenExpiry);

        // Profile picture
        Task<ResponseDto<string>> UploadProfilePictureAsync(int userId, IFormFile file);
    }
}
