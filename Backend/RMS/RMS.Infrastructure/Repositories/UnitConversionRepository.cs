using RMS.Infrastructure.IRepositories;
using RMS.Domain.Interfaces;
using RMS.Domain.Entities;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class UnitConversionRepository : BaseRepository<UnitConversion>, IUnitConversionRepository
    {
        public UnitConversionRepository(RestaurantDbContext dbContext, ITenantService tenantService) : base(dbContext, tenantService)
        {
        }
    }
}
