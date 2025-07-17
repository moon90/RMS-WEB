using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs.RoleDTOs.InputDTOs;
using RMS.Domain.DTOs.RolePermissionDTOs.OutputDTOs;
using RMS.Domain.Models.BaseModels;

namespace RMS.WebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null)
        {
            try
            {
                var result = await _roleService.GetAllRolesAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection);

                var response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Roles retrieved successfully",
                    Code = "200",
                    Data = result
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving roles.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Get role by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                return role.IsSuccess ? Ok(role) : NotFound(role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Create a new role
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleCreateDto roleCreateDto)
        {
            try
            {
                var result = await _roleService.CreateRoleAsync(roleCreateDto);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return CreatedAtAction(nameof(GetRoleById), new { id = result.Data.RoleID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
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
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Role ID mismatch.",
                        Code = "ID_MISMATCH"
                    });
                }

                var result = await _roleService.UpdateRoleAsync(roleUpdateDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Delete a role
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(id);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
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
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Role ID and Permission ID must be greater than 0.",
                        Code = "INVALID_INPUT"
                    });
                }

                // Assign the permission to the role
                var dto = new RolePermissionDto
                {
                    RoleID = roleId,
                    PermissionID = permissionId,
                    SortingOrder = sortingOrder,
                    AssignedAt = DateTime.UtcNow,
                    AssignedBy = User.Identity?.Name ?? "System"
                };
                var result = await _roleService.AssignPermissionToRoleAsync(dto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning the permission.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
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
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Role ID and Permission ID must be greater than 0.",
                        Code = "INVALID_INPUT"
                    });
                }

                // Unassign the permission from the role
                var result = await _roleService.UnassignPermissionFromRoleAsync(roleId, permissionId);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while unassigning the permission.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("{roleId}/assign-permissions")]
        public async Task<IActionResult> AssignPermissionsToRole(int roleId, [FromBody] List<int> permissionIds)
        {
            try
            {
                if (roleId <= 0)
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Role ID must be greater than 0.",
                        Code = "INVALID_INPUT"
                    });
                }

                if (permissionIds == null || !permissionIds.Any())
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "At least one permission ID must be provided.",
                        Code = "INVALID_INPUT"
                    });
                }

                var result = await _roleService.AssignPermissionsToRoleAsync(roleId, permissionIds);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning permissions.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }


        [HttpPost("{roleId}/unassign-permissions")]
        public async Task<IActionResult> UnassignPermissionsFromRole(int roleId, [FromBody] List<int> permissionIds)
        {
            try
            {
                if (roleId <= 0)
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Role ID must be greater than 0.",
                        Code = "INVALID_INPUT"
                    });
                }

                if (permissionIds == null || !permissionIds.Any())
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "At least one permission ID must be provided.",
                        Code = "INVALID_INPUT"
                    });
                }

                var result = await _roleService.UnassignPermissionsFromRoleAsync(roleId, permissionIds);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning permissions: {ex.Message}",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}
