using AutoMapper;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Infrastructure.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RMS.Application.DTOs.AuditLogs;
using System;
using System.Threading;

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
                Console.WriteLine($"Error logging audit: {ex.Message}");
            }
        }

        public async Task<KeysetPagedResult<AuditLogDto>> GetAllAuditLogsAsync(int? lastSeenId, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _readDb.AuditLogs.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    var trimmed = searchQuery.Trim();
                    query = query.Where(l => 
                        l.Action.Contains(trimmed) || 
                        l.EntityType.Contains(trimmed) || 
                        l.PerformedBy.Contains(trimmed) || 
                        (l.Details != null && l.Details.Contains(trimmed)));
                }

                if (lastSeenId.HasValue && lastSeenId.Value > 0)
                {
                    query = query.Where(l => l.Id < lastSeenId.Value);
                }

                // Enforce indexing key order
                query = query.OrderByDescending(l => l.Id);

                // Fetch pageSize + 1 items to determine if next page exists
                var items = await query.Take(pageSize + 1).ToListAsync(cancellationToken);

                bool hasNextPage = items.Count > pageSize;
                var pagedEntities = items.Take(pageSize).ToList();

                int? newLastSeenId = pagedEntities.Any() ? pagedEntities.Last().Id : null;

                var dtos = _mapper.Map<List<AuditLogDto>>(pagedEntities);

                return new KeysetPagedResult<AuditLogDto>(dtos, pageSize, hasNextPage, newLastSeenId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audit log query fallback: {ex.Message}");
                // Return empty result gracefully on empty/uninitialized table
                return new KeysetPagedResult<AuditLogDto>(new List<AuditLogDto>(), pageSize, false, null);
            }
        }

        public async Task<List<AuditLogDto>> GetAuditLogsByTypeAsync(string type)
        {
            try
            {
                var query = _readDb.AuditLogs.AsNoTracking().AsQueryable();
                query = query.Where(l => l.Action == type);
                var auditLogs = await query.OrderByDescending(l => l.PerformedAt).ToListAsync();
                return _mapper.Map<List<AuditLogDto>>(auditLogs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audit log by type fallback: {ex.Message}");
                return new List<AuditLogDto>();
            }
        }
    }
}
