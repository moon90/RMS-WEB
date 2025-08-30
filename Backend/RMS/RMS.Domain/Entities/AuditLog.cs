using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }

        public required string Action { get; set; }             // e.g., "AssignRole", "DeleteUser"
        public required string EntityType { get; set; }         // e.g., "UserRole", "Permission"
        public required string EntityId { get; set; }           // e.g., "UserId:1-RoleId:2"
        public required string PerformedBy { get; set; }        // Username or UserId
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

        public string? Details { get; set; }            // Optional: JSON/text describing change
    }
}
