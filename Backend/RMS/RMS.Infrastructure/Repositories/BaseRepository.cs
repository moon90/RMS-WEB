using Microsoft.EntityFrameworkCore;
using RMS.Domain.Extensions;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Domain.Specification;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly RestaurantDbContext _context;

        public BaseRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<PagedResult<T>> GetPagedResultAsync(PagedQuery param, Expression<Func<T, object>>? orderByExpression = null, bool isDescending = false, IQueryable<T>? queryableInput = null)
        {
            var query = queryableInput ?? _context.Set<T>();

            if (orderByExpression != null)
            {
                query = isDescending ? query.OrderByDescending(orderByExpression) : query.OrderBy(orderByExpression);
            }

            return await query.ToPagedList(param.PageNumber, param.PageSize);
        }

        public async Task<IEnumerable<T>> GetOrderedAsync(BaseSpecification<T> specs)
        {
            return await ApplySpecification(specs).ToListAsync();
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<IEnumerable<T>> GetBySpecAsync(BaseSpecification<T> specs)
        {
            return await ApplySpecification(specs).ToListAsync();
        }

        public async Task<bool> ExistsAsync(BaseSpecification<T> spec)
        {
            return await ApplySpecification(spec).AnyAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AnyAsync(expression);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var query = _context.Set<T>().AsQueryable();

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply Includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            query = spec.IncludesStrings.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
