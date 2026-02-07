using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(object id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<PagedResult<T>> GetPagedResultAsync(PagedQuery param, Expression<Func<T, object>>? orderByExpression = null, bool isDescending = false, IQueryable<T>? queryableInput = null);
        Task<IEnumerable<T>> GetOrderedAsync(BaseSpecification<T> specs);
        IQueryable<T> GetQueryable();
        Task<IEnumerable<T>> GetBySpecAsync(BaseSpecification<T> specs);
        Task<bool> ExistsAsync(BaseSpecification<T> spec);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
    }
}
