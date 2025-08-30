
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(RestaurantDbContext context) : base(context)
        {
        }
    }
}
