using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Page number and page size must be greater than 0.",
                        Code = "INVALID_PAGINATION"
                    });
                }

                var result = await _userService.GetAllUsersAsync(pageNumber, pageSize);
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Users retrieved successfully",
                    Code = "200",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving users.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("{id:int}")]
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
        public async Task<IActionResult> AssignRoleToUser(int userId, int roleId)
        {
            try
            {
                var result = await _userService.AssignRoleToUserAsync(userId, roleId);
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
        public async Task<IActionResult> UploadProfilePicture(int userId, IFormFile file)
        {
            try
            {
                var result = await _userService.UploadProfilePictureAsync(userId, file);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred during upload.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}

