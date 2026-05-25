using RMS.Infrastructure.IRepositories;
using RMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(RestaurantDbContext context, ITenantService tenantService) : base(context, tenantService)
        {
        }

        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
        }
    }
}
