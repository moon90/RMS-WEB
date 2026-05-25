using RMS.Infrastructure.IRepositories;
using RMS.Domain.Extensions;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Domain.Specification;
using RMS.Infrastructure.Persistences;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly RestaurantDbContext _context;
        protected readonly ITenantService _tenantService;

        public BaseRepository(RestaurantDbContext context, ITenantService tenantService)
        {
            _context = context;
            _tenantService = tenantService;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            // Security Check: If entity is IMultiTenant, ensure it belongs to the current branch
            if (entity is IMultiTenant multiTenantEntity && _tenantService.BranchID.HasValue)
            {
                if (multiTenantEntity.BranchID != _tenantService.BranchID.Value)
                    return null; // Return null if user attempts to fetch entity from another branch
            }

            return entity;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await GetQueryable().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            // Automatic Multi-Tenant Assignment
            if (entity is IMultiTenant multiTenantEntity && _tenantService.BranchID.HasValue)
            {
                if (!multiTenantEntity.BranchID.HasValue) // Only set if not already manually specified
                {
                    multiTenantEntity.BranchID = _tenantService.BranchID.Value;
                }
            }

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
            var query = _context.Set<T>().AsQueryable();

            // Automatic Multi-Tenant Filtering
            if (typeof(IMultiTenant).IsAssignableFrom(typeof(T)) && _tenantService.BranchID.HasValue)
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var property = Expression.Property(parameter, nameof(IMultiTenant.BranchID));
                var value = Expression.Constant(_tenantService.BranchID, typeof(int?));
                var body = Expression.Equal(property, value);
                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

                query = query.Where(lambda);
            }

            return query;
        }

        public IQueryable<T> GetQueryableIgnoreTenantFilters()
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
            return await GetQueryable().AnyAsync(expression);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var query = GetQueryable();

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
