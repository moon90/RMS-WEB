using RMS.Infrastructure.IRepositories;
using RMS.Domain.Interfaces;
using RMS.Domain.Entities;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class ProductIngredientRepository : BaseRepository<ProductIngredient>, IProductIngredientRepository
    {
        public ProductIngredientRepository(RestaurantDbContext context, ITenantService tenantService) : base(context, tenantService)
        {
        }
    }
}
