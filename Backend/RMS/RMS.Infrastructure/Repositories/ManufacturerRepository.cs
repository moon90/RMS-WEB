
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class ManufacturerRepository : BaseRepository<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(RestaurantDbContext context) : base(context)
        {
        }
    }
}
