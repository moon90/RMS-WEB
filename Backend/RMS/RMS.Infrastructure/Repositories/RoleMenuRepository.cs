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

        public async Task<RoleMenu> GetRoleMenuByIdAsync(int roleMenuId)
        {
            return await _context.RoleMenus.FindAsync(roleMenuId);
        }

        public async Task<IEnumerable<RoleMenu>> GetRoleMenusByRoleIdAsync(int roleId)
        {
            return await _context.RoleMenus
                .Where(rm => rm.RoleID == roleId)
                .Include(rm => rm.Menu)
                .ToListAsync();
        }

        public async Task<IEnumerable<RoleMenu>> GetRoleMenusByMenuIdAsync(int menuId)
        {
            return await _context.RoleMenus
                .Where(rm => rm.MenuID == menuId)
                .ToListAsync();
        }

        public async Task AddRoleMenuAsync(RoleMenu roleMenu)
        {
            await _context.RoleMenus.AddAsync(roleMenu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleMenuAsync(RoleMenu roleMenu)
        {
            _context.RoleMenus.Update(roleMenu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleMenuAsync(int roleMenuId)
        {
            var roleMenu = await _context.RoleMenus.FindAsync(roleMenuId);
            if (roleMenu != null)
            {
                _context.RoleMenus.Remove(roleMenu);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsMenuAssignedToRoleAsync(int roleId, int menuId)
        {
            return await _context.RoleMenus
                .AnyAsync(rm => rm.RoleID == roleId && rm.MenuID == menuId);
        }

        public async Task AssignMenuToRoleAsync(int roleId, int menuId, bool canView, bool canAdd, bool canEdit, bool canDelete)
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

        public async Task UnassignMenuFromRoleAsync(int roleId, int menuId)
        {
            var roleMenu = await _context.RoleMenus
                .FirstOrDefaultAsync(rm => rm.RoleID == roleId && rm.MenuID == menuId);

            if (roleMenu != null)
            {
                _context.RoleMenus.Remove(roleMenu);
                await _context.SaveChangesAsync();
            }
        }
    }
}
