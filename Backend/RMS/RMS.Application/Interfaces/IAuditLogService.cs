
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
        Task<PagedResult<AuditLogDto>> GetAllAuditLogsAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection);
        Task<List<AuditLogDto>> GetAuditLogsByTypeAsync(string type);
    }
}
