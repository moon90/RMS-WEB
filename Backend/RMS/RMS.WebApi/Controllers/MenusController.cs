using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Domain.DTOs.MenuDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleMenuDTOs.OutputDTOs;

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
        public async Task<IActionResult> GetMenuById(int id)
        {
            try
            {
                var menu = await _menuService.GetMenuByIdAsync(id);
                if (menu == null)
                    return NotFound();

                return Ok(menu);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the menu: {ex.Message}");
            }
        }

        // Get all menus with pagination
        [HttpGet]
        public async Task<IActionResult> GetAllMenus([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate pagination parameters
                if (pageNumber < 1 || pageSize < 1)
                    return BadRequest("Page number and page size must be greater than 0.");

                // Get paginated menus
                var pagedResult = await _menuService.GetAllMenusAsync(pageNumber, pageSize);

                // Return paginated response
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving menus: {ex.Message}");
            }
        }

        // Create a new menu
        [HttpPost]
        public async Task<IActionResult> CreateMenu(MenuCreateDto menuCreateDto)
        {
            try
            {
                var menuId = await _menuService.CreateMenuAsync(menuCreateDto);
                return CreatedAtAction(nameof(GetMenuById), new { id = menuId }, new { MenuId = menuId });
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
                return StatusCode(500, $"An error occurred while creating the menu: {ex.Message}");
            }
        }

        // Update an existing menu
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, MenuUpdateDto menuUpdateDto)
        {
            try
            {
                // Ensure the ID in the URL matches the ID in the DTO
                if (id != menuUpdateDto.MenuID)
                    return BadRequest("Menu ID mismatch.");

                await _menuService.UpdateMenuAsync(menuUpdateDto);
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
                return StatusCode(500, $"An error occurred while updating the menu: {ex.Message}");
            }
        }

        // Delete a menu
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                await _menuService.DeleteMenuAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the menu: {ex.Message}");
            }
        }

        // Assign a menu to a role with permissions
        [HttpPost("{roleId}/menus/{menuId}")]
        public async Task<IActionResult> AssignMenuToRole(int roleId, int menuId, [FromBody] RoleMenuDto menuPermissionDto)
        {
            try
            {
                // Validate roleId and menuId
                if (roleId <= 0 || menuId <= 0)
                    return BadRequest("Role ID and Menu ID must be greater than 0.");

                // Assign the menu to the role with permissions
                await _menuService.AssignMenuToRoleAsync(menuPermissionDto);

                return Ok("Menu assigned to role successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while assigning the menu: {ex.Message}");
            }
        }

        // Unassign a menu from a role
        [HttpDelete("{roleId}/menus/{menuId}")]
        public async Task<IActionResult> UnassignMenuFromRole(int roleId, int menuId)
        {
            try
            {
                // Validate roleId and menuId
                if (roleId <= 0 || menuId <= 0)
                    return BadRequest("Role ID and Menu ID must be greater than 0.");

                // Unassign the menu from the role
                await _menuService.UnassignMenuFromRoleAsync(roleId, menuId);

                return Ok("Menu unassigned from role successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while unassigning the menu: {ex.Message}");
            }
        }
    }
}
