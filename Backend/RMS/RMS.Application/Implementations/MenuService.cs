using AutoMapper;
using FluentValidation;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.DTOs.MenuDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleMenuDTOs.OutputDTOs;
using RMS.Domain.Entities;
using RMS.Domain.Models.BaseModels;
using RMS.Infrastructure.Interfaces;

namespace RMS.Application.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<MenuCreateDto> _menuCreateValidator;
        private readonly IValidator<MenuUpdateDto> _menuUpdateValidator;

        public MenuService(
            IMenuRepository menuRepository,
            IRoleMenuRepository roleMenuRepository,
            IMapper mapper,
            IValidator<MenuCreateDto> menuCreateValidator,
            IValidator<MenuUpdateDto> menuUpdateValidator)
        {
            _menuRepository = menuRepository;
            _roleMenuRepository = roleMenuRepository;
            _roleMenuRepository = roleMenuRepository;
            _mapper = mapper;
            _menuCreateValidator = menuCreateValidator;
            _menuUpdateValidator = menuUpdateValidator;
        }

        // Get menu by ID
        public async Task<ResponseDto<MenuDto>> GetMenuByIdAsync(int menuId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);

            if (menu == null)
            {
                return new ResponseDto<MenuDto>
                {
                    IsSuccess = false,
                    Message = "Menu not found.",
                    Code = "404"
                };
            }

            var menuDto = _mapper.Map<MenuDto>(menu);

            return new ResponseDto<MenuDto>
            {
                IsSuccess = true,
                Message = "Menu retrieved successfully.",
                Code = "200",
                Data = menuDto
            };
        }

        // Get all menus
        public async Task<ResponseDto<IEnumerable<MenuDto>>> GetAllMenusAsync()
        {
            var menus = await _menuRepository.GetAllMenusAsync();

            if (menus == null || !menus.Any())
            {
                return new ResponseDto<IEnumerable<MenuDto>>
                {
                    IsSuccess = false,
                    Message = "No menus found.",
                    Code = "204"
                };
            }

            var menuDtos = _mapper.Map<IEnumerable<MenuDto>>(menus);

            return new ResponseDto<IEnumerable<MenuDto>>
            {
                IsSuccess = true,
                Message = "Menus retrieved successfully.",
                Code = "200",
                Data = menuDtos
            };
        }

        public async Task<PagedResult<MenuDto>> GetAllMenusAsync(int pageNumber, int pageSize)
        {
            var (menus, totalCount) = await _menuRepository.GetAllMenusAsync(pageNumber, pageSize);
            var menuDtos = _mapper.Map<List<MenuDto>>(menus);

            // Wrap in PagedResult
            var pagedResult = new PagedResult<MenuDto>(menuDtos, pageNumber, pageSize, totalCount);
            return pagedResult;
        }

        // Create a new menu
        public async Task<ResponseDto<int>> CreateMenuAsync(MenuCreateDto menuCreateDto)
        {
            var validationResult = await _menuCreateValidator.ValidateAsync(menuCreateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<int>
                {
                    IsSuccess = false,
                    Message = "Validation failed.",
                    Code = "400",
                    Data = 0,
                    Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                };
            }

            var menu = _mapper.Map<Menu>(menuCreateDto);
            await _menuRepository.AddMenuAsync(menu);

            return new ResponseDto<int>
            {
                IsSuccess = true,
                Message = "Menu created successfully.",
                Code = "201",
                Data = menu.Id
            };
        }


        // Update an existing menu
        public async Task<ResponseDto<object>> UpdateMenuAsync(MenuUpdateDto menuUpdateDto)
        {
            var validationResult = await _menuUpdateValidator.ValidateAsync(menuUpdateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Validation failed.",
                    Code = "400",
                    Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                };
            }

            var menu = await _menuRepository.GetMenuByIdAsync(menuUpdateDto.MenuID);
            if (menu == null)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Menu not found.",
                    Code = "404",
                    Details = new { menuUpdateDto.MenuID }
                };
            }

            _mapper.Map(menuUpdateDto, menu);
            await _menuRepository.UpdateMenuAsync(menu);

            return new ResponseDto<object>
            {
                IsSuccess = true,
                Message = "Menu updated successfully.",
                Code = "200",
            };
        }

        // Delete a menu
        public async Task<ResponseDto<object>> DeleteMenuAsync(int menuId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            if (menu == null)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Menu not found.",
                    Code = "404",
                    Details = new { MenuId = menuId }
                };
            }

            await _menuRepository.DeleteMenuAsync(menuId);

            return new ResponseDto<object>
            {
                IsSuccess = true,
                Message = "Menu deleted successfully.",
                Code = "200"
            };
        }


        public async Task<ResponseDto<object>> AssignMenuToRoleAsync(RoleMenuDto roleMenuDto)
        {
            // (Optional) Validate input manually or use FluentValidation if applicable
            if (roleMenuDto.RoleID <= 0 || roleMenuDto.MenuID <= 0)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Invalid RoleID or MenuID.",
                    Code = "400",
                    Details = new { roleMenuDto.RoleID, roleMenuDto.MenuID }
                };
            }

            // (Optional) Check if role and menu exist before assignment
            var roleExists = await _roleRepository.RoleExistsAsync(roleMenuDto.RoleID);
            var menuExists = await _menuRepository.MenuExistsAsync(roleMenuDto.MenuID);

            if (!roleExists || !menuExists)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Role or Menu not found.",
                    Code = "404",
                    Details = new { roleExists, menuExists }
                };
            }

            await _roleMenuRepository.AssignMenuToRoleAsync(
                roleMenuDto.RoleID,
                roleMenuDto.MenuID,
                roleMenuDto.CanView,
                roleMenuDto.CanAdd,
                roleMenuDto.CanEdit,
                roleMenuDto.CanDelete
            );

            return new ResponseDto<object>
            {
                IsSuccess = true,
                Message = "Menu successfully assigned to role.",
                Code = "200"
            };
        }


        // Unassign a menu from a role
        public async Task<ResponseDto<object>> UnassignMenuFromRoleAsync(int roleId, int menuId)
        {
            // Validate input IDs
            if (roleId <= 0 || menuId <= 0)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Invalid RoleID or MenuID.",
                    Code = "400",
                    Data = null,
                    Details = new { roleId, menuId }
                };
            }

            // Optional: Check if the assignment exists before attempting to unassign
            var exists = await _roleMenuRepository.IsMenuAssignedToRoleAsync(roleId, menuId);
            if (!exists)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Menu is not assigned to the role.",
                    Code = "404",
                    Details = new { roleId, menuId }
                };
            }

            await _roleMenuRepository.UnassignMenuFromRoleAsync(roleId, menuId);

            return new ResponseDto<object>
            {
                IsSuccess = true,
                Message = "Menu unassigned from role successfully.",
                Code = "200",
            };
        }

    }
}
