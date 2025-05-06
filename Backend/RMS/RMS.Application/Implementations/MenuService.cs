using AutoMapper;
using FluentValidation;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.Interfaces;
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
            _mapper = mapper;
            _menuCreateValidator = menuCreateValidator;
            _menuUpdateValidator = menuUpdateValidator;
        }

        // Get menu by ID
        public async Task<MenuDto> GetMenuByIdAsync(int menuId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            if (menu == null)
                throw new ArgumentException("Menu not found.");

            return _mapper.Map<MenuDto>(menu);
        }

        // Get all menus
        public async Task<IEnumerable<MenuDto>> GetAllMenusAsync()
        {
            var menus = await _menuRepository.GetAllMenusAsync();
            return _mapper.Map<IEnumerable<MenuDto>>(menus);
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
        public async Task<int> CreateMenuAsync(MenuCreateDto menuCreateDto)
        {
            // Validate input
            var validationResult = await _menuCreateValidator.ValidateAsync(menuCreateDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var menu = _mapper.Map<Menu>(menuCreateDto);
            await _menuRepository.AddMenuAsync(menu);
            return menu.Id;
        }

        // Update an existing menu
        public async Task UpdateMenuAsync(MenuUpdateDto menuUpdateDto)
        {
            // Validate input
            var validationResult = await _menuUpdateValidator.ValidateAsync(menuUpdateDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var menu = await _menuRepository.GetMenuByIdAsync(menuUpdateDto.MenuID);
            if (menu == null)
                throw new ArgumentException("Menu not found.");

            _mapper.Map(menuUpdateDto, menu);
            await _menuRepository.UpdateMenuAsync(menu);
        }

        // Delete a menu
        public async Task DeleteMenuAsync(int menuId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            if (menu == null)
                throw new ArgumentException("Menu not found.");

            await _menuRepository.DeleteMenuAsync(menuId);
        }

        // Assign a menu to a role with permissions
        public async Task AssignMenuToRoleAsync(RoleMenuDto roleMenuDto)
        {
            await _roleMenuRepository.AssignMenuToRoleAsync(roleMenuDto.RoleID, roleMenuDto.MenuID, roleMenuDto.CanView, roleMenuDto.CanAdd, roleMenuDto.CanEdit, roleMenuDto.CanDelete);
        }

        // Unassign a menu from a role
        public async Task UnassignMenuFromRoleAsync(int roleId, int menuId)
        {
            await _roleMenuRepository.UnassignMenuFromRoleAsync(roleId, menuId);
        }
    }
}
