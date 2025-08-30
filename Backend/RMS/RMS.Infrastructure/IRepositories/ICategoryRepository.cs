using RMS.Domain.Entities;

namespace RMS.Infrastructure.IRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> GetCategoryByNameAsync(string categoryName);
        IQueryable<Category> ApplySearch(IQueryable<Category> query, string? searchQuery);
    }
}
