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

        public async Task<UserRole> GetUserRoleByIdAsync(int userRoleId)
        {
            return await _context.UserRoles.FindAsync(userRoleId);
        }

        public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserID == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserRole>> GetUserRolesByRoleIdAsync(int roleId)
        {
            return await _context.UserRoles
                .Where(ur => ur.RoleID == roleId)
                .ToListAsync();
        }

        public async Task AddUserRoleAsync(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserRoleAsync(UserRole userRole)
        {
            _context.UserRoles.Update(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserRoleAsync(int userRoleId)
        {
            var userRole = await _context.UserRoles.FindAsync(userRoleId);
            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsRoleAssignedToUserAsync(int userId, int roleId)
        {
            return await _context.UserRoles
                .AnyAsync(ur => ur.UserID == userId && ur.RoleID == roleId);
        }

        public async Task AssignRoleToUserAsync(int userId, int roleId)
        {
            if (!await IsRoleAssignedToUserAsync(userId, roleId))
            {
                var userRole = new UserRole
                {
                    UserID = userId,
                    RoleID = roleId
                };
                await _context.UserRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnassignRoleFromUserAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserID == userId && ur.RoleID == roleId);

            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();
            }
        }
    }
}
