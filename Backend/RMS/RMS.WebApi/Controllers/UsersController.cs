using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Implementations;
using RMS.Application.Interfaces;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.Domain.Entities;

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

        // Get all users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate pagination parameters
                if (pageNumber < 1 || pageSize < 1)
                    return BadRequest("Page number and page size must be greater than 0.");

                // Get paginated users
                var pagedResult = await _userService.GetAllUsersAsync(pageNumber, pageSize);

                // Return paginated response
                //var response = new
                //{
                //    TotalCount = totalCount,
                //    PageNumber = pageNumber,
                //    PageSize = pageSize,
                //    Users = users
                //};

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving users: {ex.Message}");
            }
        }

        // Get user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the user: {ex.Message}");
            }
        }

        // Get user by UserName
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(username);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the user: {ex.Message}");
            }
        }

        // Create a new user
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto)
        {
            try
            {
                var userId = await _userService.CreateUserAsync(userCreateDto);
                return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { UserId = userId });
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the user: {ex.Message}");
            }
        }

        // Update an existing user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userUpdateDto)
        {
            try
            {
                if (id != userUpdateDto.UserID)
                    return BadRequest("User ID mismatch.");

                await _userService.UpdateUserAsync(userUpdateDto);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the user: {ex.Message}");
            }
        }

        // Delete a user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
            }
        }

        // Assign a role to a user
        [HttpPost("{userId}/roles/{roleId}")]
        public async Task<IActionResult> AssignRoleToUser(int userId, int roleId)
        {
            try
            {
                await _userService.AssignRoleToUserAsync(userId, roleId);
                return Ok("Role assigned successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while assigning the role: {ex.Message}");
            }
        }

        // Unassign a role from a user
        [HttpDelete("{userId}/roles/{roleId}")]
        public async Task<IActionResult> UnassignRoleFromUser(int userId, int roleId)
        {
            try
            {
                await _userService.UnassignRoleFromUserAsync(userId, roleId);
                return Ok("Role unassigned successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while unassigning the role: {ex.Message}");
            }
        }

        [HttpPost("{userId}/assign-roles")]
        public async Task<IActionResult> AssignRolesToUser(int userId, [FromBody] List<int> roleIds)
        {
            try
            {
                string performedBy = User.Identity?.Name ?? "System";

                if (userId <= 0)
                    return BadRequest("User ID must be greater than 0.");

                if (roleIds == null || !roleIds.Any())
                    return BadRequest("At least one role ID must be provided.");

                await _userService.AssignRolesToUserAsync(userId, roleIds, performedBy);
                return Ok("Roles assigned to role successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while assigning permissions: {ex.Message}");
            }
        }

        [HttpPost("{userId}/unassign-roles")]
        public async Task<IActionResult> UnassignRolesFromUser(int userId, [FromBody] List<int> roleIds)
        {
            try
            {
                string performedBy = User.Identity?.Name ?? "System";

                if (userId <= 0)
                    return BadRequest("User ID must be greater than 0.");

                if (roleIds == null || !roleIds.Any())
                    return BadRequest("At least one role ID must be provided.");

                await _userService.UnassignRolesFromUserAsync(userId, roleIds, performedBy);
                return Ok("Roles unassigned.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while assigning permissions: {ex.Message}");
            }                        
        }
    }
}
