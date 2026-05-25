using RMS.Infrastructure.IRepositories;
using RMS.Domain.Interfaces;
using RMS.Domain.Entities;
using RMS.Infrastructure.Persistences;


namespace RMS.Infrastructure.Repositories
{
    public class SaleRepository : BaseRepository<Sale>, ISaleRepository
    {
        public SaleRepository(RestaurantDbContext dbContext, ITenantService tenantService) : base(dbContext, tenantService)
        {
        }
    }
}
