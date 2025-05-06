using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Implementations
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repository;

        public AuditLogService(IAuditLogRepository repository)
        {
            _repository = repository;
        }

        public async Task LogAsync(string action, string entityType, string entityId, string performedBy, string details = null)
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
    }
}
