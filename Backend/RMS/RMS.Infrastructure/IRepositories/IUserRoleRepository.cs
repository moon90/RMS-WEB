using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<UserRole> GetUserRoleByIdAsync(int userRoleId);
        Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(int userId);
        Task<IEnumerable<UserRole>> GetUserRolesByRoleIdAsync(int roleId);
        Task AddUserRoleAsync(UserRole userRole);
        Task UpdateUserRoleAsync(UserRole userRole);
        Task DeleteUserRoleAsync(int userRoleId);

        // New methods for role assignment
        Task<bool> IsRoleAssignedToUserAsync(int userId, int roleId);
        Task AssignRoleToUserAsync(int userId, int roleId);
        Task UnassignRoleFromUserAsync(int userId, int roleId);
    }
}
