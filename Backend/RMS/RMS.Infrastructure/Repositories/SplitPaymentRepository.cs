using RMS.Infrastructure.IRepositories;
using RMS.Domain.Interfaces;
using RMS.Domain.Entities;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class SplitPaymentRepository : BaseRepository<SplitPayment>, ISplitPaymentRepository
    {
        public SplitPaymentRepository(RestaurantDbContext context, ITenantService tenantService) : base(context, tenantService)
        {
        }
    }
}
