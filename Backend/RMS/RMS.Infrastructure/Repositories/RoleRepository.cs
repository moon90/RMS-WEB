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
            try
            {
                return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving role by name: {ex.Message}");
                throw; 
            }
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            try
            {
                return await _context.Roles.FindAsync(roleId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving role by ID: {ex.Message}");
                throw; 
            }
        }

        public async Task<(IEnumerable<Role> Roles, int TotalCount)> GetAllRolesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all roles: {ex.Message}");
                throw; 
            }
        }

        public async Task AddRoleAsync(Role role)
        {
            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding role: {ex.Message}");
                throw; 
            }
        }

        public async Task UpdateRoleAsync(Role role)
        {
            try
            {
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating role: {ex.Message}");
                throw; 
            }
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            try
            {
                var role = await _context.Roles.FindAsync(roleId);
                if (role != null)
                {
                    _context.Roles.Remove(role);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting role: {ex.Message}");
                throw; 
            }
        }

        public async Task<bool> RoleExistsAsync(int roleId)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.Id == roleId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if role exists: {ex.Message}");
                throw; 
            }
        }

    }
}
