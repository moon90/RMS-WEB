using AutoMapper;
using AutoMapper.QueryableExtensions;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Infrastructure.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RMS.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using RMS.Application.DTOs;
using RMS.Core.Enum;
using RMS.Application.DTOs.AuditLogs;

namespace RMS.Application.Implementations
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repository;
        private readonly RMS.Infrastructure.Persistences.ReadOnlyRestaurantDbContext _readDb;
        private readonly IMapper _mapper;

        public AuditLogService(IAuditLogRepository repository, RMS.Infrastructure.Persistences.ReadOnlyRestaurantDbContext readDb, IMapper mapper)
        {
            _repository = repository;
            _readDb = readDb;
            _mapper = mapper;
        }

        public async Task LogAsync(string action, string entityType, string entityId, string performedBy, string details = null)
        {
            try
            {
                var log = new AuditLog
                {
                    Action = action,
                    EntityType = entityType,
                    EntityId = entityId,
                    PerformedBy = performedBy,
                    PerformedAt = DateTime.UtcNow,
                    Details = details
                };

                await _repository.AddAsync(log);
            }
            catch (Exception ex)
            {
                // Log the exception internally, e.g., to a file or a monitoring system.
                // For this exercise, we'll just prevent it from crashing the application.
                Console.WriteLine($"Error logging audit: {ex.Message}");
            }
        }

        public async Task<KeysetPagedResult<AuditLogDto>> GetAllAuditLogsAsync(int? lastSeenId, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, CancellationToken cancellationToken = default)
        {
            var query = _readDb.AuditLogs.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(l => l.Action.Contains(searchQuery) || l.EntityType.Contains(searchQuery) || l.PerformedBy.Contains(searchQuery) || (l.Details != null && l.Details.Contains(searchQuery)));
            }

            // For Keyset Pagination, we enforce sorting by the indexed Id
            query = query.OrderByDescending(l => l.Id);

            var projectedQuery = query.ProjectTo<AuditLogDto>(_mapper.ConfigurationProvider);
            return await projectedQuery.ToKeysetPagedList(l => l.Id, lastSeenId, pageSize, cancellationToken);
        }

        public async Task<List<AuditLogDto>> GetAuditLogsByTypeAsync(string type)
        {
            var query = _readDb.AuditLogs.AsQueryable();
            query = query.Where(l => l.Action == type);
            var auditLogs = await query.OrderByDescending(l => l.PerformedAt).ToListAsync();
            return _mapper.Map<List<AuditLogDto>>(auditLogs);
        }
    }
}
