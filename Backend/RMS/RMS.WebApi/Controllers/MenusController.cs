using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using RMS.Application.DTOs.MenuDTOs.InputDTOs;
using RMS.Application.DTOs.RoleMenuDTOs.OutputDTOs;
using RMS.Application.DTOs.RoleMenuDTOs.InputDTOs;
using RMS.Domain.Models.BaseModels;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // Get menu by ID
        [HttpGet("{id}")]
        [Authorize(Policy = "MENU_VIEW")]
        public async Task<ActionResult<ResponseDto<MenuDto>>> GetMenuById(int id)
        {
            try
            {
                var response = await _menuService.GetMenuByIdAsync(id);
                if (!response.IsSuccess)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<MenuDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the menu.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Get all menus with pagination
        [HttpGet]
        [Authorize(Policy = "MENU_VIEW")]
        public async Task<ActionResult<PagedResult<MenuDto>>> GetAllMenus([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Page number and page size must be greater than 0.",
                        Code = "400"
                    });
                }

                var pagedResult = await _menuService.GetAllMenusAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving menus.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Create a new menu
        [HttpPost]
        [Authorize(Policy = "MENU_CREATE")]
        public async Task<ActionResult<ResponseDto<int>>> CreateMenu(MenuCreateDto menuCreateDto)
        {
            try
            {
                var response = await _menuService.CreateMenuAsync(menuCreateDto);
                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }
                return CreatedAtAction(nameof(GetMenuById), new { id = response.Data }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<int>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the menu.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Update an existing menu
        [HttpPut("{id}")]
        [Authorize(Policy = "MENU_UPDATE")]
        public async Task<ActionResult<ResponseDto<object>>> UpdateMenu(int id, MenuUpdateDto menuUpdateDto)
        {
            try
            {
                if (id != menuUpdateDto.MenuID)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Menu ID mismatch.",
                        Code = "400"
                    });
                }

                var response = await _menuService.UpdateMenuAsync(menuUpdateDto);
                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the menu.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Delete a menu
        [HttpDelete("{id}")]
        [Authorize(Policy = "MENU_DELETE")]
        public async Task<ActionResult<ResponseDto<object>>> DeleteMenu(int id)
        {
            try
            {
                var response = await _menuService.DeleteMenuAsync(id);
                if (!response.IsSuccess)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the menu.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Assign a menu to a role with permissions
        [HttpPost("{roleId}/menus/{menuId}")]
        [Authorize(Policy = "MENU_ASSIGN_ROLE")]
        public async Task<ActionResult<ResponseDto<object>>> AssignMenuToRole(int roleId, int menuId, [FromBody] RoleMenuDto menuPermissionDto)
        {
            try
            {
                if (roleId <= 0 || menuId <= 0)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Role ID and Menu ID must be greater than 0.",
                        Code = "400"
                    });
                }

                var response = await _menuService.AssignMenuToRoleAsync(menuPermissionDto);
                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning the menu to the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Unassign a menu from a role
        [HttpDelete("{roleId}/menus/{menuId}")]
        [Authorize(Policy = "MENU_UNASSIGN_ROLE")]
        public async Task<ActionResult<ResponseDto<object>>> UnassignMenuFromRole(int roleId, int menuId)
        {
            try
            {
                if (roleId <= 0 || menuId <= 0)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Role ID and Menu ID must be greater than 0.",
                        Code = "400"
                    });
                }

                var response = await _menuService.UnassignMenuFromRoleAsync(roleId, menuId);
                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while unassigning the menu from the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Get menus by role ID
        [HttpGet("role/{roleId}")]
        [Authorize(Policy = "MENU_VIEW")] // Assuming MENU_VIEW is sufficient for this
        public async Task<ActionResult<ResponseDto<IEnumerable<RoleMenuDto>>>> GetMenusByRoleId(int roleId)
        {
            try
            {
                if (roleId <= 0)
                {
                    return BadRequest(new ResponseDto<IEnumerable<RoleMenuDto>>
                    {
                        IsSuccess = false,
                        Message = "Role ID must be greater than 0.",
                        Code = "400"
                    });
                }

                var response = await _menuService.GetMenusByRoleIdAsync(roleId);
                if (!response.IsSuccess)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<IEnumerable<RoleMenuDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving menus for the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("{roleId}/assign-menus")]
        [Authorize(Policy = "MENU_ASSIGN_ROLE")] // Or a new policy like "MENU_BULK_ASSIGN_ROLE"
        public async Task<ActionResult<ResponseDto<object>>> AssignMenusToRole(int roleId, [FromBody] RoleMenuBulkUpdateDto dto)
        {
            try
            {
                if (roleId <= 0 || dto == null || roleId != dto.RoleId || dto.MenuIds == null || !dto.MenuIds.Any())
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Invalid Role ID or Menu IDs provided.",
                        Code = "400"
                    });
                }

                var response = await _menuService.AssignMenusToRoleAsync(dto);
                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning menus to the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("{roleId}/unassign-menus")]
        [Authorize(Policy = "MENU_UNASSIGN_ROLE")] // Or a new policy like "MENU_BULK_UNASSIGN_ROLE"
        public async Task<ActionResult<ResponseDto<object>>> UnassignMenusFromRole(int roleId, [FromBody] RoleMenuBulkUpdateDto dto)
        {
            try
            {
                if (roleId <= 0 || dto == null || roleId != dto.RoleId || dto.MenuIds == null || !dto.MenuIds.Any())
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Invalid Role ID or Menu IDs provided.",
                        Code = "400"
                    });
                }

                var response = await _menuService.UnassignMenusFromRoleAsync(dto);
                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while unassigning menus from the role.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}
