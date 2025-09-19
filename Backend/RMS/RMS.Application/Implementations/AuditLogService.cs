using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuditLogService(IAuditLogRepository repository, IMapper mapper)
        {
            _repository = repository;
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

        public async Task<PagedResult<AuditLogDto>> GetAllAuditLogsAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection)
        {
            var query = _repository.GetQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(l => l.Action.Contains(searchQuery) || l.EntityType.Contains(searchQuery) || l.PerformedBy.Contains(searchQuery) || (l.Details != null && l.Details.Contains(searchQuery)));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.ApplySort(sortColumn, sortDirection ?? "asc");
            }
            else
            {
                query = query.OrderByDescending(l => l.PerformedAt);
            }

            var pagedResult = await query.ToPagedList(pageNumber, pageSize);
            var auditLogDtos = _mapper.Map<List<AuditLogDto>>(pagedResult.Items);

            return new PagedResult<AuditLogDto>
            {
                Items = auditLogDtos,
                TotalRecords = pagedResult.TotalRecords,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };
        }

        public async Task<List<AuditLogDto>> GetAuditLogsByTypeAsync(string type)
        {
            var query = _repository.GetQueryable();
            query = query.Where(l => l.Action == type);
            var auditLogs = await query.OrderByDescending(l => l.PerformedAt).ToListAsync();
            return _mapper.Map<List<AuditLogDto>>(auditLogs);
        }
    }
}
