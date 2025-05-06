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

        public async Task<Menu> GetMenuByIdAsync(int menuId)
        {
            return await _context.Menus.FindAsync(menuId);
        }

        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            return await _context.Menus.ToListAsync();
        }

        public async Task<(IEnumerable<Menu> Menus, int TotalCount)> GetAllMenusAsync(int pageNumber, int pageSize)
        {
            var query = _context.Menus.AsQueryable();

            // Calculate the number of items to skip
            var skip = (pageNumber - 1) * pageSize;

            // Get the total count of roles
            var totalCount = await query.CountAsync();

            // Apply pagination
            var menus = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return (menus, totalCount);
        }

        public async Task<IEnumerable<Menu>> GetMenusByParentIdAsync(int? parentId)
        {
            return await _context.Menus
                .Where(m => m.ParentID == parentId)
                .ToListAsync();
        }

        public async Task AddMenuAsync(Menu menu)
        {
            await _context.Menus.AddAsync(menu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMenuAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMenuAsync(int menuId)
        {
            var menu = await _context.Menus.FindAsync(menuId);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
        }
    }
}
