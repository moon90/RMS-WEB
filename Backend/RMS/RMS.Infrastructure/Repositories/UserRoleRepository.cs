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
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly RestaurantDbContext _context;

        public UserRoleRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<UserRole?> GetUserRoleByIdAsync(int userRoleId)
        {
            try
            {
                return await _context.UserRoles.FindAsync(userRoleId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user role by ID: {ex.Message}");
                throw; 
            }
        }

        public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(int userId)
        {
            try
            {
                return await _context.UserRoles
                    .Where(ur => ur.UserID == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user roles by user ID: {ex.Message}");
                throw; 
            }
        }

        public async Task<IEnumerable<UserRole>> GetUserRolesByRoleIdAsync(int roleId)
        {
            try
            {
                return await _context.UserRoles
                    .Where(ur => ur.RoleID == roleId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user roles by role ID: {ex.Message}");
                throw; 
            }
        }

        public async Task AddUserRoleAsync(UserRole userRole)
        {
            try
            {
                await _context.UserRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user role: {ex.Message}");
                throw; 
            }
        }

        public async Task UpdateUserRoleAsync(UserRole userRole)
        {
            try
            {
                _context.UserRoles.Update(userRole);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user role: {ex.Message}");
                throw; 
            }
        }

        public async Task DeleteUserRoleAsync(int userRoleId)
        {
            try
            {
                var userRole = await _context.UserRoles.FindAsync(userRoleId);
                if (userRole != null)
                {
                    _context.UserRoles.Remove(userRole);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user role: {ex.Message}");
                throw; 
            }
        }

        public async Task<bool> IsRoleAssignedToUserAsync(int userId, int roleId)
        {
            try
            {
                return await _context.UserRoles
                    .AnyAsync(ur => ur.UserID == userId && ur.RoleID == roleId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if role is assigned to user: {ex.Message}");
                throw; 
            }
        }

        public async Task AssignRoleToUserAsync(int userId, int roleId, string performedBy)
        {
            try
            {
                if (!await IsRoleAssignedToUserAsync(userId, roleId))
                {
                    var userRole = new UserRole
                    {
                        UserID = userId,
                        RoleID = roleId,
                        AssignedAt = DateTime.UtcNow,
                        AssignedBy = performedBy
                    };
                    await _context.UserRoles.AddAsync(userRole);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning role to user: {ex.Message}");
                throw; 
            }
        }

        public async Task UnassignRoleFromUserAsync(int userId, int roleId)
        {
            try
            {
                var userRole = await _context.UserRoles
                    .FirstOrDefaultAsync(ur => ur.UserID == userId && ur.RoleID == roleId);

                if (userRole != null)
                {
                    _context.UserRoles.Remove(userRole);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unassigning role from user: {ex.Message}");
                throw; 
            }
        }
    }
}
