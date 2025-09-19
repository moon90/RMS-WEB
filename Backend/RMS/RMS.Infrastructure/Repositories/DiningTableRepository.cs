using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories; // Correct interface namespace
using RMS.Infrastructure.Persistences;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Models.BaseModels; // For Result<T>
using RMS.Domain.Queries; // For PagedResult<T>
using RMS.Domain.Specification; // For BaseSpecification<T>
using System.Linq; // For .Where()

namespace RMS.Infrastructure.Repositories
{
    public class DiningTableRepository : BaseRepository<DiningTable>, IDiningTableRepository
    {
        public DiningTableRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DiningTable>> GetAvailableTablesAsync()
        {
            return await _context.Set<DiningTable>().Where(dt => dt.Status == true).ToListAsync();
        }
    }
}