
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task<Inventory> GetByProductIdAsync(int productId)
        {
            return await _context.Inventory.FirstOrDefaultAsync(x => x.ProductID == productId);
        }
    }
}

