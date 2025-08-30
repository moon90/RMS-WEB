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
            try
            {
                await _context.AuditLogs.AddAsync(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding audit log: {ex.Message}");
                throw; // Re-throw the exception for the service layer to handle
            }
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            try
            {
                return await _context.AuditLogs.OrderByDescending(x => x.PerformedAt).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all audit logs: {ex.Message}");
                throw; // Re-throw the exception for the service layer to handle
            }
        }

        public async Task<IEnumerable<AuditLog>> GetByEntityTypeAsync(string entityType)
        {
            try
            {
                return await _context.AuditLogs
                    .Where(x => x.EntityType == entityType)
                    .OrderByDescending(x => x.PerformedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving audit logs by entity type: {ex.Message}");
                throw; // Re-throw the exception for the service layer to handle
            }
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(string performedBy)
        {
            try
            {
                return await _context.AuditLogs
                    .Where(x => x.PerformedBy == performedBy)
                    .OrderByDescending(x => x.PerformedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving audit logs by user: {ex.Message}");
                throw; // Re-throw the exception for the service layer to handle
            }
        }
    }
}
