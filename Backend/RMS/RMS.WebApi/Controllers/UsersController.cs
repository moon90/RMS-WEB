using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.WebApi.Services;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IImageService _imageService; // Added

        public UsersController(IUserService userService, IImageService imageService) // Modified
        {
            _userService = userService;
            _imageService = imageService; // Added
        }

        [HttpGet]
        [Authorize(Policy = "USER_VIEW")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, [FromQuery] bool? status = null, [FromQuery] string? role = null)
        {
            try
            {
                var result = await _userService.GetAllUsersAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status, role);

                var response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Users retrieved successfully",
                    Code = "200",
                    Data = result
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving users.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message // Consider logging the full exception for debugging
                });
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "USER_VIEW")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the user.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("username/{username}")]
        [Authorize(Policy = "USER_VIEW")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            try
            {
                var result = await _userService.GetUserByUsernameAsync(username);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<UserDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the user.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Policy = "USER_CREATE")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            try
            {
                var result = await _userService.CreateUserAsync(dto);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return CreatedAtAction(nameof(GetUserById), new { id = result.Data.UserID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the user.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "USER_UPDATE")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            try
            {
                if (id != dto.UserID)
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "User ID mismatch.",
                        Code = "ID_MISMATCH"
                    });
                }

                var result = await _userService.UpdateUserAsync(dto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the user.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "USER_DELETE")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the user.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("{userId}/roles/{roleId}")]
        [Authorize(Policy = "USER_ASSIGN_ROLE")]
        public async Task<IActionResult> AssignRoleToUser(int userId, int roleId)
        {
            try
            {
                var performedBy = User.Identity?.Name ?? "System";
                var result = await _userService.AssignRoleToUserAsync(userId, roleId, performedBy);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{userId}/roles/{roleId}")]
        [Authorize(Policy = "USER_UNASSIGN_ROLE")]
        public async Task<IActionResult> UnassignRoleFromUser(int userId, int roleId)
        {
            try
            {
                var result = await _userService.UnassignRoleFromUserAsync(userId, roleId);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while unassigning the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("{userId}/assign-roles")]
        [Authorize(Policy = "USER_ASSIGN_ROLES")]
        public async Task<IActionResult> AssignRolesToUser(int userId, [FromBody] List<int> roleIds)
        {
            try
            {
                if (userId <= 0 || roleIds == null || !roleIds.Any())
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "User ID and role list must be valid.",
                        Code = "INVALID_INPUT"
                    });
                }

                var performedBy = User.Identity?.Name ?? "System";
                var result = await _userService.AssignRolesToUserAsync(userId, roleIds, performedBy);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning roles.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("{userId}/unassign-roles")]
        [Authorize(Policy = "USER_UNASSIGN_ROLES")]
        public async Task<IActionResult> UnassignRolesFromUser(int userId, [FromBody] List<int> roleIds)
        {
            try
            {
                if (userId <= 0 || roleIds == null || !roleIds.Any())
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "User ID and role list must be valid.",
                        Code = "INVALID_INPUT"
                    });
                }

                var performedBy = User.Identity?.Name ?? "System";
                var result = await _userService.UnassignRolesFromUserAsync(userId, roleIds, performedBy);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while unassigning roles.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("{userId}/upload-profile-picture")]
        [Authorize(Policy = "USER_UPLOAD_PROFILE_PICTURE")]
        public async Task<IActionResult> UploadProfilePicture(int userId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "No file uploaded.",
                        Code = "NO_FILE_UPLOADED"
                    });
                }

                // Get existing user to retrieve old profile picture URL
                var existingUserResult = await _userService.GetUserByIdAsync(userId);
                if (!existingUserResult.IsSuccess || existingUserResult.Data == null)
                {
                    return NotFound(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "User not found.",
                        Code = "USER_NOT_FOUND"
                    });
                }
                var existingUser = existingUserResult.Data;

                // Save new profile picture
                var newProfilePictureUrl = await _imageService.SaveImageAsync(file, "profile-pictures");

                // If saving is successful, delete the old profile picture
                if (!string.IsNullOrEmpty(newProfilePictureUrl) && !string.IsNullOrEmpty(existingUser.ProfilePictureUrl))
                {
                    _imageService.DeleteImage(existingUser.ProfilePictureUrl);
                }

                // Update user's profile picture URL in the database
                var result = await _userService.UploadProfilePictureAsync(userId, newProfilePictureUrl);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (ArgumentException ex) // Catch specific validation exception from ImageService
            {
                return BadRequest(new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Code = "IMAGE_VALIDATION_ERROR",
                    Details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred during profile picture upload.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("{userId}/menu-permissions")]
        [Authorize(Policy = "USER_VIEW_MENU_PERMISSIONS")]
        public async Task<IActionResult> GetMenuPermissions(int userId)
        {
            try
            {
                var result = await _userService.GetMenuPermissionsAsync(userId);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving menu permissions.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}

