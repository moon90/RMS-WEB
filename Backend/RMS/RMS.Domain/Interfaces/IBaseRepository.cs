using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(bool trackChanges = true, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task<PagedResult<T>> GetPagedResultAsync(PagedQuery param, Expression<Func<T, object>>? orderByExpression = null, bool isDescending = false, IQueryable<T>? queryableInput = null, bool trackChanges = true, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetOrderedAsync(BaseSpecification<T> specs, bool trackChanges = true, CancellationToken cancellationToken = default);
        IQueryable<T> GetQueryable(bool trackChanges = true);
        IQueryable<T> GetQueryableIgnoreTenantFilters(bool trackChanges = true);
        Task<IEnumerable<T>> GetBySpecAsync(BaseSpecification<T> specs, bool trackChanges = true, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(BaseSpecification<T> spec, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    }
}
