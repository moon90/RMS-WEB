using RMS.Domain.Entities;
using System.Threading.Tasks;

namespace RMS.Infrastructure.IRepositories
{
    public interface IPromotionRepository : IBaseRepository<Promotion>
    {
        Task<Promotion> GetPromotionByCouponCodeAsync(string couponCode);
    }
}