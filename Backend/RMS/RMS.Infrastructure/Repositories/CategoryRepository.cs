using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;

namespace RMS.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly RestaurantDbContext _context;

        public CategoryRepository(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
        }

        public IQueryable<Category> ApplySearch(IQueryable<Category> query, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(c => c.CategoryName.Contains(searchQuery));
            }
            return query;
        }
    }
}
