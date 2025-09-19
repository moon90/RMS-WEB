using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class SplitPaymentRepository : BaseRepository<SplitPayment>, ISplitPaymentRepository
    {
        public SplitPaymentRepository(RestaurantDbContext context) : base(context)
        {
        }
    }
}
