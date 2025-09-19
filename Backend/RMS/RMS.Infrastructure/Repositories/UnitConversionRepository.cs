
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class UnitConversionRepository : BaseRepository<UnitConversion>, IUnitConversionRepository
    {
        public UnitConversionRepository(RestaurantDbContext dbContext) : base(dbContext)
        {
        }
    }
}
