using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories; // Correct interface namespace
using RMS.Infrastructure.Persistences;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // For .Where()

namespace RMS.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(RestaurantDbContext context) : base(context)
        {
        }

        public new async Task<Order> GetByIdAsync(object id)
        {
            return await _context.Set<Order>() // Use _context.Set<Order>()
                                 .Include(o => o.OrderDetails)
                                 .ThenInclude(od => od.Product)
                                 .FirstOrDefaultAsync(o => o.OrderID == (int)id);
        }

        public new async Task<List<Order>> GetAllAsync()
        {
            return await _context.Set<Order>() // Use _context.Set<Order>()
                                 .Include(o => o.OrderDetails)
                                 .ThenInclude(od => od.Product)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Set<Order>() // Use _context.Set<Order>()
                                 .Where(o => o.CustomerID == customerId)
                                 .Include(o => o.OrderDetails)
                                 .ThenInclude(od => od.Product)
                                 .ToListAsync();
        }
    }
}