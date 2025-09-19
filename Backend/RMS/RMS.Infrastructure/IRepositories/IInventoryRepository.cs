using RMS.Domain.Entities;
using System.Threading.Tasks;

namespace RMS.Infrastructure.IRepositories
{
    public interface IInventoryRepository : IBaseRepository<Inventory>
    {
        Task<Inventory> GetByProductIdAsync(int productId);
    }
}
