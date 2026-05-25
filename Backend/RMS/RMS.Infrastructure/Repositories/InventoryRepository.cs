using RMS.Infrastructure.IRepositories;
using RMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Infrastructure.Persistences;
using System.Threading.Tasks;


namespace RMS.Infrastructure.Repositories
{
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(RestaurantDbContext context, ITenantService tenantService) : base(context, tenantService)
        {
        }

        public async Task<Inventory> GetByProductIdAsync(int productId)
        {
            return await GetQueryable().FirstOrDefaultAsync(x => x.ProductID == productId);
        }
    }
}

