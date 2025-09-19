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
using RMS.Domain.Extensions;

namespace RMS.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RestaurantDbContext _context;

        public UserRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all users: {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<User> Users, int TotalCount)> GetAllUsersAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status, string? role)
        {
            try
            {
                var query = _context.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).AsQueryable();

                // Filtering
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    query = query.Where(u =>
                        u.UserName.Contains(searchQuery) ||
                        u.FullName.Contains(searchQuery) ||
                        (u.Email != null && u.Email.Contains(searchQuery)) ||
                        (u.Phone != null && u.Phone.Contains(searchQuery))
                    );
                }

                if (status.HasValue)
                {
                    query = query.Where(u => u.Status == status.Value);
                }

                if (!string.IsNullOrWhiteSpace(role))
                {
                    query = query.Where(u => u.UserRoles.Any(ur => ur.Role != null && ur.Role.RoleName == role));
                }

                // Sorting
                if (!string.IsNullOrWhiteSpace(sortColumn))
                {
                    query = query.ApplySort(sortColumn, sortDirection ?? "asc");
                }
                else
                {
                    query = query.OrderBy(u => u.Id); // Default sort if no column is specified
                }

                // Get the total count of users after filtering
                var totalCount = await query.CountAsync();

                // Apply pagination
                var users = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (Users: users, TotalCount: totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving paged users: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _context.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by ID: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == username);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by username: {ex.Message}");
                throw;
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by refresh token: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime? refreshTokenExpiry)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiry = refreshTokenExpiry;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating refresh token: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by email: {ex.Message}");
                throw;
            }
        }

        public IQueryable<User> GetQueryable()
        {
            return _context.Users.AsQueryable();
        }
    }
}
