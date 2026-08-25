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

        public async Task<T> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);

            // Security Check: If entity is IMultiTenant, ensure it belongs to the current branch
            if (entity is IMultiTenant multiTenantEntity && _tenantService.BranchID.HasValue)
            {
                if (multiTenantEntity.BranchID != _tenantService.BranchID.Value)
                    return null; // Return null if user attempts to fetch entity from another branch
            }

            return entity;
        }

        public async Task<List<T>> GetAllAsync(bool trackChanges = true, CancellationToken cancellationToken = default)
        {
            return await GetQueryable(trackChanges).ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            // Automatic Multi-Tenant Assignment
            if (entity is IMultiTenant multiTenantEntity && _tenantService.BranchID.HasValue)
            {
                if (!multiTenantEntity.BranchID.HasValue) // Only set if not already manually specified
                {
                    multiTenantEntity.BranchID = _tenantService.BranchID.Value;
                }
            }

            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask; // Keep async signature but no EF core async needed
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            await Task.CompletedTask; // Keep async signature but no EF core async needed
        }

        public async Task<PagedResult<T>> GetPagedResultAsync(PagedQuery param, Expression<Func<T, object>>? orderByExpression = null, bool isDescending = false, IQueryable<T>? queryableInput = null, bool trackChanges = true, CancellationToken cancellationToken = default)
        {
            var query = queryableInput ?? GetQueryable(trackChanges);

            if (orderByExpression != null)
            {
                query = isDescending ? query.OrderByDescending(orderByExpression) : query.OrderBy(orderByExpression);
            }

            return await query.ToPagedList(param.PageNumber, param.PageSize, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetOrderedAsync(BaseSpecification<T> specs, bool trackChanges = true, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specs, trackChanges).ToListAsync(cancellationToken);
        }

        public IQueryable<T> GetQueryable(bool trackChanges = true)
        {
            var query = _context.Set<T>().AsQueryable();

            if (!trackChanges)
                query = query.AsNoTracking();

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

        public IQueryable<T> GetQueryableIgnoreTenantFilters(bool trackChanges = true)
        {
            var query = _context.Set<T>().AsQueryable();
            if (!trackChanges)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<IEnumerable<T>> GetBySpecAsync(BaseSpecification<T> specs, bool trackChanges = true, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specs, trackChanges).ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(BaseSpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec, true).AnyAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await GetQueryable(true).AnyAsync(expression, cancellationToken);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec, bool trackChanges = true)
        {
            var query = GetQueryable(trackChanges);

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
