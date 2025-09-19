using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class AuditLogRepository : BaseRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task AddAsync(AuditLog log)
        {
            await base.AddAsync(log);
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.Set<AuditLog>().OrderByDescending(x => x.PerformedAt).ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByEntityTypeAsync(string entityType)
        {
            return await _context.Set<AuditLog>()
                .Where(x => x.EntityType == entityType)
                .OrderByDescending(x => x.PerformedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(string performedBy)
        {
            return await _context.Set<AuditLog>()
                .Where(x => x.PerformedBy == performedBy)
                .OrderByDescending(x => x.PerformedAt)
                .ToListAsync();
        }
    }
}
