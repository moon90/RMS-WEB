
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(RestaurantDbContext context) : base(context)
        {
        }
    }
}
