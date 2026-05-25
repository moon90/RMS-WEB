using RMS.Infrastructure.IRepositories;
using RMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces; // Correct interface namespace
using RMS.Infrastructure.Persistences;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // For .Where()

namespace RMS.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(RestaurantDbContext context, ITenantService tenantService) : base(context, tenantService)
        {
        }

        public new async Task<Order> GetByIdAsync(object id)
        {
            return await GetQueryable()
                                 .Include(o => o.OrderDetails)
                                 .ThenInclude(od => od.Product)
                                 .FirstOrDefaultAsync(o => o.OrderID == (int)id);
        }

        public new async Task<List<Order>> GetAllAsync()
        {
            return await GetQueryable()
                                 .Include(o => o.OrderDetails)
                                 .ThenInclude(od => od.Product)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await GetQueryable()
                                 .Where(o => o.CustomerID == customerId)
                                 .Include(o => o.OrderDetails)
                                 .ThenInclude(od => od.Product)
                                 .ToListAsync();
        }
    }
}