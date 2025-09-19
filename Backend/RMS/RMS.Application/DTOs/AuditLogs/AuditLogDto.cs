using System;

namespace RMS.Application.DTOs.AuditLogs
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string PerformedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Details { get; set; }
    }
}