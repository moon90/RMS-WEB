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
    public class RoleMenuRepository : IRoleMenuRepository
    {
        private readonly RestaurantDbContext _context;

        public RoleMenuRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<RoleMenu?> GetRoleMenuByIdAsync(int roleMenuId)
        {
            try
            {
                return await _context.RoleMenus.FindAsync(roleMenuId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving role menu by ID: {ex.Message}");
                throw; 
            }
        }

        public async Task<IEnumerable<RoleMenu>> GetRoleMenusByRoleIdAsync(int roleId)
        {
            try
            {
                return await _context.RoleMenus
                    .Where(rm => rm.RoleID == roleId)
                    .Include(rm => rm.Menu)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving role menus by role ID: {ex.Message}");
                throw; 
            }
        }

        public async Task<IEnumerable<RoleMenu>> GetRoleMenusByMenuIdAsync(int menuId)
        {
            try
            {
                return await _context.RoleMenus
                    .Where(rm => rm.MenuID == menuId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving role menus by menu ID: {ex.Message}");
                throw; 
            }
        }

        public async Task AddRoleMenuAsync(RoleMenu roleMenu)
        {
            try
            {
                await _context.RoleMenus.AddAsync(roleMenu);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding role menu: {ex.Message}");
                throw; 
            }
        }

        public async Task UpdateRoleMenuAsync(RoleMenu roleMenu)
        {
            try
            {
                _context.RoleMenus.Update(roleMenu);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating role menu: {ex.Message}");
                throw; 
            }
        }

        public async Task DeleteRoleMenuAsync(int roleMenuId)
        {
            try
            {
                var roleMenu = await _context.RoleMenus.FindAsync(roleMenuId);
                if (roleMenu != null)
                {
                    _context.RoleMenus.Remove(roleMenu);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting role menu: {ex.Message}");
                throw; 
            }
        }

        public async Task<bool> IsMenuAssignedToRoleAsync(int roleId, int menuId)
        {
            try
            {
                return await _context.RoleMenus
                    .AnyAsync(rm => rm.RoleID == roleId && rm.MenuID == menuId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if menu is assigned to role: {ex.Message}");
                throw; 
            }
        }

        public async Task AssignMenuToRoleAsync(int roleId, int menuId, bool canView, bool canAdd, bool canEdit, bool canDelete)
        {
            try
            {
                var roleMenu = await _context.RoleMenus
                    .FirstOrDefaultAsync(rm => rm.RoleID == roleId && rm.MenuID == menuId);

                if (roleMenu == null)
                {
                    roleMenu = new RoleMenu
                    {
                        RoleID = roleId,
                        MenuID = menuId,
                        CanView = canView,
                        CanAdd = canAdd,
                        CanEdit = canEdit,
                        CanDelete = canDelete
                    };
                    await _context.RoleMenus.AddAsync(roleMenu);
                }
                else
                {
                    roleMenu.CanView = canView;
                    roleMenu.CanAdd = canAdd;
                    roleMenu.CanEdit = canEdit;
                    roleMenu.CanDelete = canDelete;
                    _context.RoleMenus.Update(roleMenu);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning menu to role: {ex.Message}");
                throw; 
            }
        }

        public async Task UnassignMenuFromRoleAsync(int roleId, int menuId)
        {
            try
            {
                Console.WriteLine($"Attempting to unassign menu. RoleID: {roleId}, MenuID: {menuId}");

                var roleMenu = await _context.RoleMenus
                    .FirstOrDefaultAsync(rm => rm.RoleID == roleId && rm.MenuID == menuId);

                if (roleMenu != null)
                {
                    _context.RoleMenus.Remove(roleMenu);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Menu unassigned successfully. RoleID: {roleId}, MenuID: {menuId}");
                }
                else
                {
                    Console.WriteLine($"Menu assignment not found for unassignment. RoleID: {roleId}, MenuID: {menuId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unassigning menu from role. RoleID: {roleId}, MenuID: {menuId}. Error: {ex.Message}");
                throw; 
            }
        }

        public async Task<RoleMenu?> GetRoleMenuByRoleIdAndMenuIdAsync(int roleId, int menuId)
        {
            try
            {
                return await _context.RoleMenus
                    .FirstOrDefaultAsync(rm => rm.RoleID == roleId && rm.MenuID == menuId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving role menu by role ID and menu ID: {ex.Message}");
                throw;
            }
        }
    }
}
