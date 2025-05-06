using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RMS.Core.Exceptions;
using RMS.Core.Extensions;
using RMS.Core.Guard;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Domain.Specification;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly RestaurantDbContext _context;
        protected DbSet<T> Entities { get; }

        public BaseRepository(RestaurantDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            Entities = context.Set<T>();
        }

        #region implemtation for GET methods
        public async Task<T> GetByIdAsync(object id)
            => GuardClause.Against.NotFound(await Entities.FindAsync(id), nameof(id));

        public async Task<IEnumerable<T>> GetOrderedAsync(OrderSpecification<T> specs)
        {
            var query = Entities.AsQueryable();
            if (specs is not null && specs.OrderBy is not null)
                query = specs.IsDescending
                    ? query.OrderByDescending(specs.OrderBy)
                    : query.OrderBy(specs.OrderBy);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetOrderedAsync(BaseSpecification<T> specs)
        {
            var query = AddSpecQueryAsync(specs);

            if (specs is not null && specs.OrderBy is not null)
                query = specs.IsDescending
                    ? query.OrderByDescending(specs.OrderBy)
                    : query.OrderBy(specs.OrderBy);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetBySpecAsync(BaseSpecification<T> specs)
            => await AddSpecQueryAsync(specs).ToListAsync();

        public async Task<PagedResult<T>> GetPagedResultAsync(PagedQuery param, IQueryable<T>? queryableInput)
        {
            var queryable = queryableInput is null ? Entities.AsQueryable() : queryableInput;

            if (!string.IsNullOrEmpty(param.OrderBy))
            {
                queryable = param.IsDescending
                    ? queryable.OrderByDescending(e => EF.Property<object>(e, param.OrderBy))
                    : queryable.OrderBy(e => EF.Property<object>(e, param.OrderBy));
            }

            int totalRecords = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalRecords / param.PageSize);
            if (param.PageNumber > totalPages && totalPages > 0)
                throw new NotFoundException($"Page {param.PageNumber} does not exist. Maximum page number is {totalPages}.");

            var items = await queryable.Skip((param.PageNumber - 1) * param.PageSize)
                                       .Take(param.PageSize)
                                       .ToListAsync();

            return new PagedResult<T>(items, param.PageNumber, param.PageSize, totalRecords);
        }

        public IQueryable<T> GetQueryable()
            => Entities.AsQueryable();
        #endregion

        #region implementation for CheckExist methods
        public async Task<bool> ExistsAsync(BaseSpecification<T> spec)
            => await Entities.AnyAsync(spec.Criteria);

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
            => await Entities.AnyAsync(expression);
        #endregion

        #region implementation for ADD methos
        public async Task AddAsync(T entity)
            => await Entities.AddAsync(entity);
        #endregion


        public Task<Result> AddRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<T>> GetFirstBySpecAsync(BaseSpecification<T> specs)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetFirstOrDefaultBySpecAsync(BaseSpecification<T> specs)
        {
            var query = AddSpecQueryAsync(specs);

            return await query.FirstOrDefaultAsync();
        }

        public Task<Result> RemoveAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<Result> RemoveByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<Result> RemoveBySpecAsync(BaseSpecification<T> specification)
        {
            throw new NotImplementedException();
        }


        public Task<Result> UpdateRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        #region private methods
        private IQueryable<T> AddSpecQueryAsync(BaseSpecification<T> specs)
        {
            var query = Entities.AsQueryable();

            if (specs.Criteria.IsNotNull())
                query = query.Where(specs.Criteria);

            return query;
        }

        Task<Result<T>> IBaseRepository<T>.GetFirstOrDefaultBySpecAsync(BaseSpecification<T> specs)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
