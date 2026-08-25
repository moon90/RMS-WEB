
using RMS.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Application.DTOs.AuditLogs;

namespace RMS.Application.Interfaces
{
    public interface IAuditLogService
    {
        Task LogAsync(string action, string entityType, string entityId, string performedBy, string? details = null);
        Task<KeysetPagedResult<AuditLogDto>> GetAllAuditLogsAsync(int? lastSeenId, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, CancellationToken cancellationToken = default);
        Task<List<AuditLogDto>> GetAuditLogsByTypeAsync(string type);
    }
}
