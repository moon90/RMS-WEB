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

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<(IEnumerable<Role> Roles, int TotalCount)> GetAllRolesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection)
        {
            var query = _context.Roles.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(r => r.RoleName.Contains(searchQuery));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn.ToLower())
                {
                    case "rolename":
                        query = sortDirection?.ToLower() == "desc" ? query.OrderByDescending(r => r.RoleName) : query.OrderBy(r => r.RoleName);
                        break;
                    case "id":
                        query = sortDirection?.ToLower() == "desc" ? query.OrderByDescending(r => r.Id) : query.OrderBy(r => r.Id);
                        break;
                    default:
                        query = query.OrderBy(r => r.Id); // Default sort
                        break;
                }
            }
            else
            {
                query = query.OrderBy(r => r.Id); // Default sort if no column is specified
            }

            var totalCount = await query.CountAsync();

            var roles = await query
                .Skip((pageNumber - 1) * pageSize)
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
