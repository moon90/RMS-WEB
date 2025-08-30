using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.Interfaces;
using RMS.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly RestaurantDbContext _context;

        public MenuRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<Menu?> GetMenuByIdAsync(int menuId)
        {
            try
            {
                return await _context.Menus.FindAsync(menuId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving menu by ID: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }

        public async Task<Menu?> GetMenuByNameAsync(string menuName)
        {
            try
            {
                return await _context.Menus.FirstOrDefaultAsync(m => m.MenuName == menuName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving menu by name: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            try
            {
                return await _context.Menus.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all menus: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }

        public async Task<(IEnumerable<Menu> Menus, int TotalCount)> GetAllMenusAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection)
        {
            try
            {
                var query = _context.Menus.AsQueryable();

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    query = query.Where(m => m.MenuName.Contains(searchQuery) ||
                                             m.MenuPath.Contains(searchQuery) ||
                                             m.MenuIcon.Contains(searchQuery) ||
                                             m.ControllerName.Contains(searchQuery) ||
                                             m.ActionName.Contains(searchQuery) ||
                                             m.ModuleName.Contains(searchQuery));
                }

                // Apply sorting
                if (!string.IsNullOrWhiteSpace(sortColumn))
                {
                    switch (sortColumn.ToLower())
                    {
                        case "menuname":
                            query = sortDirection?.ToLower() == "desc" ? query.OrderByDescending(m => m.MenuName) : query.OrderBy(m => m.MenuName);
                            break;
                        case "menupath":
                            query = sortDirection?.ToLower() == "desc" ? query.OrderByDescending(m => m.MenuPath) : query.OrderBy(m => m.MenuPath);
                            break;
                        case "menuicon":
                            query = sortDirection?.ToLower() == "desc" ? query.OrderByDescending(m => m.MenuIcon) : query.OrderBy(m => m.MenuIcon);
                            break;
                        case "controllername":
                            query = sortDirection?.ToLower() == "desc" ? query.OrderByDescending(m => m.ControllerName) : query.OrderBy(m => m.ControllerName);
                            break;
                        case "actionname":
                            query = sortDirection?.ToLower() == "desc" ? query.OrderByDescending(m => m.ActionName) : query.OrderBy(m => m.ActionName);
                            break;
                        case "modulename":
                            query = sortDirection?.ToLower() == "desc" ? query.OrderByDescending(m => m.ModuleName) : query.OrderBy(m => m.ModuleName);
                            break;
                        case "displayorder":
                            query = sortDirection?.ToLower() == "desc" ? query.OrderByDescending(m => m.DisplayOrder) : query.OrderBy(m => m.DisplayOrder);
                            break;
                        default:
                            query = query.OrderBy(m => m.Id); // Default sort
                            break;
                    }
                }
                else
                {
                    query = query.OrderBy(m => m.Id); // Default sort if no column is specified
                }

                // Get the total count of menus after filtering
                var totalCount = await query.CountAsync();

                // Apply pagination
                var menus = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (menus, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving paged menus: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }

        public async Task<IEnumerable<Menu>> GetMenusByParentIdAsync(int? parentId)
        {
            try
            {
                return await _context.Menus
                    .Where(m => m.ParentID == parentId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving menus by parent ID: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }

        public async Task AddMenuAsync(Menu menu)
        {
            try
            {
                await _context.Menus.AddAsync(menu);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding menu: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }

        public async Task UpdateMenuAsync(Menu menu)
        {
            try
            {
                _context.Menus.Update(menu);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating menu: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }

        public async Task DeleteMenuAsync(int menuId)
        {
            try
            {
                var menu = await _context.Menus.FindAsync(menuId);
                if (menu != null)
                {
                    _context.Menus.Remove(menu);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting menu: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }

        public async Task<bool> MenuExistsAsync(int menuId)
        {
            try
            {
                return await _context.Menus.AnyAsync(m => m.Id == menuId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if menu exists: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }
    }
}
