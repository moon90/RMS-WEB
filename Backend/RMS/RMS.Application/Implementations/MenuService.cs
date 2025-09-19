using AutoMapper;
using FluentValidation;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using RMS.Application.DTOs.MenuDTOs.InputDTOs;
using RMS.Application.DTOs.RoleMenuDTOs.OutputDTOs;
using RMS.Domain.Entities;
using RMS.Domain.Models.BaseModels;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Infrastructure.Interfaces;
using RMS.Application.DTOs.RoleMenuDTOs.InputDTOs;

namespace RMS.Application.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<MenuCreateDto> _menuCreateValidator;
        private readonly IValidator<MenuUpdateDto> _menuUpdateValidator;

        public MenuService(
            IMenuRepository menuRepository,
            IRoleRepository roleRepository,
            IRoleMenuRepository roleMenuRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<MenuCreateDto> menuCreateValidator,
            IValidator<MenuUpdateDto> menuUpdateValidator)
        {
            _menuRepository = menuRepository;
            _roleRepository = roleRepository;
            _roleMenuRepository = roleMenuRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _menuCreateValidator = menuCreateValidator;
            _menuUpdateValidator = menuUpdateValidator;
        }

        // Get menu by ID
        public async Task<ResponseDto<MenuDto>> GetMenuByIdAsync(int menuId)
        {
            try
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
            catch (Exception ex)
            {
                return new ResponseDto<MenuDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the menu.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        // Get all menus
        public async Task<ResponseDto<IEnumerable<MenuDto>>> GetAllMenusAsync()
        {
            try
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
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<MenuDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving menus.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<PagedResult<MenuDto>> GetAllMenusAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection)
        {
            try
            {
                var (menus, totalCount) = await _menuRepository.GetAllMenusAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection);
                var menuDtos = _mapper.Map<List<MenuDto>>(menus);

                // Wrap in PagedResult
                var pagedResult = new PagedResult<MenuDto>(menuDtos, pageNumber, pageSize, totalCount);
                return pagedResult;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetAllMenusAsync (paged): {ex.Message}");
                throw; // Re-throw or handle as appropriate for your application's error handling strategy
            }
        }

        // Create a new menu
        public async Task<ResponseDto<int>> CreateMenuAsync(MenuCreateDto menuCreateDto)
        {
            try
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

                // Check if a menu with the same name already exists
                var existingMenu = await _menuRepository.GetMenuByNameAsync(menuCreateDto.MenuName);
                if (existingMenu != null)
                {
                    return new ResponseDto<int>
                    {
                        IsSuccess = false,
                        Message = "Menu name already exists.",
                        Code = "409",
                        Data = 0
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
            catch (Exception ex)
            {
                return new ResponseDto<int>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the menu.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        // Update an existing menu
        public async Task<ResponseDto<object>> UpdateMenuAsync(MenuUpdateDto menuUpdateDto)
        {
            try
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

                // Check if a menu with the same name already exists (excluding the current menu being updated)
                var existingMenuByName = await _menuRepository.GetMenuByNameAsync(menuUpdateDto.MenuName);
                if (existingMenuByName != null && existingMenuByName.Id != menuUpdateDto.MenuID)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Menu name already exists.",
                        Code = "409"
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
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the menu.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        // Delete a menu
        public async Task<ResponseDto<object>> DeleteMenuAsync(int menuId)
        {
            try
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
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the menu.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        public async Task<ResponseDto<object>> AssignMenuToRoleAsync(RoleMenuDto roleMenuDto)
        {
            try
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
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning the menu to the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        // Unassign a menu from a role
        public async Task<ResponseDto<object>> UnassignMenuFromRoleAsync(int roleId, int menuId)
        {
            try
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
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while unassigning the menu from the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<IEnumerable<RoleMenuDto>>> GetMenusByRoleIdAsync(int roleId)
        {
            try
            {
                var roleMenus = await _roleMenuRepository.GetRoleMenusByRoleIdAsync(roleId);

                if (roleMenus == null || !roleMenus.Any())
                {
                    return new ResponseDto<IEnumerable<RoleMenuDto>>
                    {
                        IsSuccess = false,
                        Message = "No menus found for this role.",
                        Code = "204"
                    };
                }

                var roleMenuDtos = _mapper.Map<IEnumerable<RoleMenuDto>>(roleMenus);

                return new ResponseDto<IEnumerable<RoleMenuDto>>
                {
                    IsSuccess = true,
                    Message = "Menus retrieved successfully for the role.",
                    Code = "200",
                    Data = roleMenuDtos
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<RoleMenuDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving menus for the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<IEnumerable<UserMenuPermissionDto>>> GetUserMenuPermissionsAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new ResponseDto<IEnumerable<UserMenuPermissionDto>>
                    {
                        IsSuccess = false,
                        Message = "User not found.",
                        Code = "404"
                    };
                }

                var allMenus = await _menuRepository.GetAllMenusAsync();
                var userRoles = user.UserRoles.Select(ur => ur.RoleID).ToList();

                var userMenuPermissions = new List<UserMenuPermissionDto>();

                foreach (var menu in allMenus)
                {
                    bool canView = false;
                    bool canAdd = false;
                    bool canEdit = false;
                    bool canDelete = false;

                    foreach (var roleId in userRoles)
                    {
                        var roleMenu = await _roleMenuRepository.GetRoleMenuByRoleIdAndMenuIdAsync(roleId, menu.Id);
                        if (roleMenu != null)
                        {
                            canView = canView || roleMenu.CanView;
                            canAdd = canAdd || roleMenu.CanAdd;
                            canEdit = canEdit || roleMenu.CanEdit;
                            canDelete = canDelete || roleMenu.CanDelete;
                        }
                    }

                    if (canView || canAdd || canEdit || canDelete) // Only add if user has any permission
                    {
                        var userMenuPermission = _mapper.Map<UserMenuPermissionDto>(menu);
                        userMenuPermission.CanView = canView;
                        userMenuPermission.CanAdd = canAdd;
                        userMenuPermission.CanEdit = canEdit;
                        userMenuPermission.CanDelete = canDelete;
                        userMenuPermissions.Add(userMenuPermission);
                    }
                }

                return new ResponseDto<IEnumerable<UserMenuPermissionDto>>
                {
                    IsSuccess = true,
                    Message = "User menu permissions retrieved successfully.",
                    Code = "200",
                    Data = userMenuPermissions
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<UserMenuPermissionDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving user menu permissions.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<object>> AssignMenusToRoleAsync(RoleMenuBulkUpdateDto dto)
        {
            try
            {
                if (dto.RoleId <= 0 || dto.MenuIds == null || !dto.MenuIds.Any())
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Invalid RoleID or MenuIDs.",
                        Code = "400",
                        Details = new { dto.RoleId, dto.MenuIds }
                    };
                }

                var roleExists = await _roleRepository.RoleExistsAsync(dto.RoleId);
                if (!roleExists)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Role not found.",
                        Code = "404",
                        Details = new { dto.RoleId }
                    };
                }

                foreach (var menuId in dto.MenuIds)
                {
                    var menuExists = await _menuRepository.MenuExistsAsync(menuId);
                    if (menuExists)
                    {
                        await _roleMenuRepository.AssignMenuToRoleAsync(dto.RoleId, menuId, dto.CanView, dto.CanAdd, dto.CanEdit, dto.CanDelete);
                    }
                }

                return new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Menus successfully assigned to role.",
                    Code = "200"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while assigning menus to the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<object>> UnassignMenusFromRoleAsync(RoleMenuBulkUpdateDto dto)
        {
            try
            {
                if (dto.RoleId <= 0 || dto.MenuIds == null || !dto.MenuIds.Any())
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Invalid RoleID or MenuIDs.",
                        Code = "400",
                        Details = new { dto.RoleId, dto.MenuIds }
                    };
                }

                var roleExists = await _roleRepository.RoleExistsAsync(dto.RoleId);
                if (!roleExists)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Role not found.",
                        Code = "404",
                        Details = new { dto.RoleId }
                    };
                }

                foreach (var menuId in dto.MenuIds)
                {
                    var menuExists = await _menuRepository.MenuExistsAsync(menuId);
                    if (menuExists)
                    {
                        await _roleMenuRepository.UnassignMenuFromRoleAsync(dto.RoleId, menuId);
                    }
                }

                return new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Menus successfully unassigned from role.",
                    Code = "200"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while unassigning menus from the role.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

    }
}
