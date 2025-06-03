using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.Interfaces;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RestaurantDbContext _context;

        public RoleRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<(IEnumerable<Role> Roles, int TotalCount)> GetAllRolesAsync(int pageNumber, int pageSize)
        {
            var query = _context.Roles.AsQueryable();

            // Calculate the number of items to skip
            var skip = (pageNumber - 1) * pageSize;

            // Get the total count of roles
            var totalCount = await query.CountAsync();

            // Apply pagination
            var roles = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return (roles, totalCount);
        }

        public async Task AddRoleAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> RoleExistsAsync(int roleId)
        {
            return await _context.Roles.AnyAsync(r => r.Id == roleId);
        }

    }
}
