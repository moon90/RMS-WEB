using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class ProductIngredientRepository : BaseRepository<ProductIngredient>, IProductIngredientRepository
    {
        public ProductIngredientRepository(RestaurantDbContext context) : base(context)
        {
        }
    }
}
