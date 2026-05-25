using RMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
﻿using RMS.Domain.Interfaces;

namespace RMS.Infrastructure.Repositories
{
    public class AuditLogRepository : BaseRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(RestaurantDbContext context, ITenantService tenantService) : base(context, tenantService)
        {
        }

        public async Task AddAsync(AuditLog log)
        {
            await base.AddAsync(log);
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await GetQueryable().OrderByDescending(x => x.PerformedAt).ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByEntityTypeAsync(string entityType)
        {
            return await GetQueryable()
                .Where(x => x.EntityType == entityType)
                .OrderByDescending(x => x.PerformedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(string performedBy)
        {
            return await GetQueryable()
                .Where(x => x.PerformedBy == performedBy)
                .OrderByDescending(x => x.PerformedAt)
                .ToListAsync();
        }
    }
}
