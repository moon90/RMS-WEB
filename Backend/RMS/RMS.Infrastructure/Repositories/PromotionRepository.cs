using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
    {
        public PromotionRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task<Promotion> GetPromotionByCouponCodeAsync(string couponCode)
        {
            return await _context.Promotions.FirstOrDefaultAsync(p => p.CouponCode == couponCode);
        }
    }
}