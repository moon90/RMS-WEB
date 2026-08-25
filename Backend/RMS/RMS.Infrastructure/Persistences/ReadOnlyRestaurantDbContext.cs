using Microsoft.EntityFrameworkCore;

namespace RMS.Infrastructure.Persistences
{
    public class ReadOnlyRestaurantDbContext : RestaurantDbContext
    {
        public ReadOnlyRestaurantDbContext(DbContextOptions<ReadOnlyRestaurantDbContext> options, RMS.Domain.Interfaces.ITenantService tenantService)
            : base(options, tenantService)
        {
            // By disabling tracking globally, we ensure this context is ONLY used for fast reads.
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
