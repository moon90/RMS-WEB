using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public int Id { get; set; }
        // Display name for UI or logs
        public required string PermissionName { get; set; }  // e.g., "Create User", "Delete Product"

        // Unique permission key used internally or in policies
        public required string PermissionKey { get; set; }   // e.g., "USER_CREATE", "PRODUCT_DELETE"

        // Optional: maps to Controller and Action (for dynamic enforcement or auditing)
        public string? ControllerName { get; set; } // e.g., "User"
        public string? ActionName { get; set; }     // e.g., "Create"

        // Optional: to categorize permissions or group by module
        public string? ModuleName { get; set; }     // e.g., "User Management"

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

}
