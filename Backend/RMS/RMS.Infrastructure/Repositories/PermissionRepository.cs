using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Infrastructure.Interfaces;
using RMS.Infrastructure.Persistences;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly RestaurantDbContext _context;

        public PermissionRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<Permission?> GetPermissionByIdAsync(int permissionId)
        {
            try
            {
                return await _context.Permissions.FindAsync(permissionId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving permission by ID: {ex.Message}");
                throw; 
            }
        }

        public async Task<Permission?> GetPermissionByNameAsync(string permissionName)
        {
            try
            {
                return await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionName == permissionName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving permission by name: {ex.Message}");
                throw;
            }
        }

        public async Task<Permission?> GetPermissionByKeyAsync(string permissionKey)
        {
            try
            {
                return await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionKey == permissionKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving permission by key: {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<Permission> Permissions, int TotalCount)> GetAllPermissionsAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            try
            {
                var query = _context.Permissions.AsQueryable();

                // Apply search query
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    query = query.Where(p => p.PermissionName.Contains(searchQuery) || p.PermissionKey.Contains(searchQuery));
                }

                // Apply status filter
                if (status.HasValue)
                {
                    query = query.Where(p => p.Status == status.Value);
                }

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply sorting
                if (!string.IsNullOrWhiteSpace(sortColumn))
                {
                    switch (sortColumn.ToLower())
                    {
                        case "permissionname":
                            query = sortDirection == "asc" ? query.OrderBy(p => p.PermissionName) : query.OrderByDescending(p => p.PermissionName);
                            break;
                        case "permissionkey":
                            query = sortDirection == "asc" ? query.OrderBy(p => p.PermissionKey) : query.OrderByDescending(p => p.PermissionKey);
                            break;
                        case "modulename":
                            query = sortDirection == "asc" ? query.OrderBy(p => p.ModuleName) : query.OrderByDescending(p => p.ModuleName);
                            break;
                        case "createddate":
                            query = sortDirection == "asc" ? query.OrderBy(p => p.CreatedDate) : query.OrderByDescending(p => p.CreatedDate);
                            break;
                        default:
                            query = query.OrderBy(p => p.Id); // Default sort
                            break;
                    }
                }
                else
                {
                    query = query.OrderBy(p => p.Id); // Default sort if no column specified
                }

                // Apply pagination
                var permissions = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (permissions, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all permissions: {ex.Message}");
                throw;
            }
        }

        public async Task AddPermissionAsync(Permission permission)
        {
            try
            {
                await _context.Permissions.AddAsync(permission);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding permission: {ex.Message}");
                throw; 
            }
        }

        public async Task UpdatePermissionAsync(Permission permission)
        {
            try
            {
                _context.Permissions.Update(permission);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating permission: {ex.Message}");
                throw; 
            }
        }

        public async Task DeletePermissionAsync(int permissionId)
        {
            try
            {
                var permission = await _context.Permissions.FindAsync(permissionId);
                if (permission != null)
                {
                    _context.Permissions.Remove(permission);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting permission: {ex.Message}");
                throw; 
            }
        }
    }
}
