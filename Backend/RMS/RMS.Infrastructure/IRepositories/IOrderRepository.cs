using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories; // Correct namespace for IBaseRepository

namespace RMS.Infrastructure.IRepositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        // Add any Order-specific methods here that are not covered by IBaseRepository
        // For example:
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
    }
}