using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Domain.DTOs.RoleDTOs.InputDTOs;
using RMS.Domain.DTOs.RolePermissionDTOs.OutputDTOs;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Get all roles with pagination
        [HttpGet]
        public async Task<IActionResult> GetAllRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate pagination parameters
                if (pageNumber < 1 || pageSize < 1)
                    return BadRequest("Page number and page size must be greater than 0.");

                // Get paginated roles
                var pagedResult = await _roleService.GetAllRolesAsync(pageNumber, pageSize);

                // Return paginated response
                //var response = new
                //{
                //    TotalCount = totalCount,
                //    PageNumber = pageNumber,
                //    PageSize = pageSize,
                //    Roles = roles
                //};

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving roles: {ex.Message}");
            }
        }

        // Get role by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                    return NotFound();

                return Ok(role);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the role: {ex.Message}");
            }
        }

        // Create a new role
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleCreateDto roleCreateDto)
        {
            try
            {
                var roleId = await _roleService.CreateRoleAsync(roleCreateDto);
                return CreatedAtAction(nameof(GetRoleById), new { id = roleId }, new { RoleId = roleId });
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
                return StatusCode(500, $"An error occurred while creating the role: {ex.Message}");
            }
        }

        // Update an existing role
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, RoleUpdateDto roleUpdateDto)
        {
            try
            {
                // Ensure the ID in the URL matches the ID in the DTO
                if (id != roleUpdateDto.RoleID)
                    return BadRequest("Role ID mismatch.");

                await _roleService.UpdateRoleAsync(roleUpdateDto);
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
                return StatusCode(500, $"An error occurred while updating the role: {ex.Message}");
            }
        }

        // Delete a role
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the role: {ex.Message}");
            }
        }

        // Assign a permission to a role
        [HttpPost("{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> AssignPermissionToRole(int roleId, int permissionId, [FromQuery] int? sortingOrder = null)
        {
            try
            {
                // Validate roleId and permissionId
                if (roleId <= 0 || permissionId <= 0)
                    return BadRequest("Role ID and Permission ID must be greater than 0.");

                // Assign the permission to the role
                var dto = new RolePermissionDto
                {
                    RoleID = roleId,
                    PermissionID = permissionId,
                    SortingOrder = sortingOrder,
                    AssignedAt = DateTime.UtcNow,
                    AssignedBy = User.Identity?.Name ?? "System"
                };
                await _roleService.AssignPermissionToRoleAsync(dto);

                return Ok("Permission assigned to role successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while assigning the permission: {ex.Message}");
            }
        }

        // Unassign a permission from a role
        [HttpDelete("{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> UnassignPermissionFromRole(int roleId, int permissionId)
        {
            try
            {
                // Validate roleId and permissionId
                if (roleId <= 0 || permissionId <= 0)
                    return BadRequest("Role ID and Permission ID must be greater than 0.");

                // Unassign the permission from the role
                await _roleService.UnassignPermissionFromRoleAsync(roleId, permissionId);

                return Ok("Permission unassigned from role successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while unassigning the permission: {ex.Message}");
            }
        }

        [HttpPost("{roleId}/assign-permissions")]
        public async Task<IActionResult> AssignPermissionsToRole(int roleId, [FromBody] List<int> permissionIds)
        {
            try
            {
                if (roleId <= 0)
                    return BadRequest("Role ID must be greater than 0.");

                if (permissionIds == null || !permissionIds.Any())
                    return BadRequest("At least one permission ID must be provided.");

                await _roleService.AssignPermissionsToRoleAsync(roleId, permissionIds);

                return Ok("Permissions assigned to role successfully.");
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


        [HttpPost("{roleId}/unassign-permissions")]
        public async Task<IActionResult> UnassignPermissionsFromRole(int roleId, [FromBody] List<int> permissionIds)
        {
            try
            {
                if(roleId <= 0)
                    return BadRequest("Role ID must be greater than 0.");

                if (permissionIds == null || !permissionIds.Any())
                    return BadRequest("At least one permission ID must be provided.");

                await _roleService.UnassignPermissionsFromRoleAsync(roleId, permissionIds);
                return Ok("Permissions unassigned.");
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
