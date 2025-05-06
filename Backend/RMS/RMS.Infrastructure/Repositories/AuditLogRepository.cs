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
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly RestaurantDbContext _context;

        public AuditLogRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog log)
        {
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs.OrderByDescending(x => x.PerformedAt).ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByEntityTypeAsync(string entityType)
        {
            return await _context.AuditLogs
                .Where(x => x.EntityType == entityType)
                .OrderByDescending(x => x.PerformedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(string performedBy)
        {
            return await _context.AuditLogs
                .Where(x => x.PerformedBy == performedBy)
                .OrderByDescending(x => x.PerformedAt)
                .ToListAsync();
        }
    }
}
