using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class UnitRepository : BaseRepository<Unit>, IUnitRepository
    {
        public UnitRepository(RestaurantDbContext context) : base(context)
        {
        }
    }
}
