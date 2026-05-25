using RMS.Domain.Interfaces;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.IRepositories
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        Task<UserRole?> GetUserRoleByIdAsync(int userRoleId);
        Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(int userId);
        Task<IEnumerable<UserRole>> GetUserRolesByRoleIdAsync(int roleId);
        Task AddUserRoleAsync(UserRole userRole);
        Task UpdateUserRoleAsync(UserRole userRole);
        Task DeleteUserRoleAsync(int userRoleId);

        // New methods for role assignment
        Task<bool> IsRoleAssignedToUserAsync(int userId, int roleId);
        Task AssignRoleToUserAsync(int userId, int roleId, string performedBy);
        Task UnassignRoleFromUserAsync(int userId, int roleId);
    }
}
