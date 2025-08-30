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
        public async Task<Result<T>> GetByIdAsync(object id)
        {
            try
            {
                var entity = await Entities.FindAsync(id);
                if (entity == null)
                {
                    return Result<T>.Failure($"{typeof(T).Name} with ID {id} not found.");
                }
                return Result<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error retrieving {typeof(T).Name} by ID: {ex.Message}");
            }
        }

        public async Task<IEnumerable<T>> GetOrderedAsync(OrderSpecification<T> specs)
        {
            try
            {
                var query = Entities.AsQueryable();
                if (specs is not null && specs.OrderBy is not null)
                    query = specs.IsDescending
                        ? query.OrderByDescending(specs.OrderBy)
                        : query.OrderBy(specs.OrderBy);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving ordered entities: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }

        public async Task<IEnumerable<T>> GetOrderedAsync(BaseSpecification<T> specs)
        {
            try
            {
                var query = AddSpecQueryAsync(specs);

                if (specs is not null && specs.OrderBy is not null)
                    query = specs.IsDescending
                        ? query.OrderByDescending(specs.OrderBy)
                        : query.OrderBy(specs.OrderBy);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving ordered entities by spec: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }

        public async Task<IEnumerable<T>> GetBySpecAsync(BaseSpecification<T> specs)
            => await AddSpecQueryAsync(specs).ToListAsync();

        public async Task<PagedResult<T>> GetPagedResultAsync(PagedQuery param, Expression<Func<T, object>>? orderByExpression = null, bool isDescending = false, IQueryable<T>? queryableInput = null)
        {
            var queryable = queryableInput ?? Entities.AsQueryable();

            if (orderByExpression != null)
            {
                queryable = isDescending
                    ? queryable.OrderByDescending(orderByExpression)
                    : queryable.OrderBy(orderByExpression);
            }

            int totalRecords = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalRecords / param.PageSize);
            if (param.PageNumber > totalPages && totalPages > 0)
                throw new NotFoundException($"Page {param.PageNumber} does not exist. Maximum page number is {totalPages}.");

            var items = await queryable.Skip((param.PageNumber - 1) * param.PageSize)
                                       .Take(param.PageSize)
                                       .ToListAsync();

            return new PagedResult<T>(items ?? new List<T>(), param.PageNumber, param.PageSize, totalRecords);
        }

        public IQueryable<T> GetQueryable()
            => Entities.AsQueryable();
        #endregion

        #region implementation for CheckExist methods
        public async Task<bool> ExistsAsync(BaseSpecification<T> spec)
            => spec.Criteria != null && await Entities.AnyAsync(spec.Criteria);

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
            => expression != null && await Entities.AnyAsync(expression);
        #endregion

        #region implementation for ADD methos
        public async Task AddAsync(T entity)
            => await Entities.AddAsync(entity);
        #endregion


        public async Task<Result> AddRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                await Entities.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error adding range of entities: {ex.Message}");
            }
        }

        public async Task<Result> DeleteByIdAsync(object id)
        {
            try
            {
                var entity = await Entities.FindAsync(id);
                if (entity == null)
                {
                    return Result.Failure($"{typeof(T).Name} with ID {id} not found.");
                }
                Entities.Remove(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error deleting {typeof(T).Name} by ID: {ex.Message}");
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await Entities.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all entities: {ex.Message}");
                throw; // Re-throw for service layer to handle
            }
        }

        public async Task<Result<T>> GetFirstBySpecAsync(BaseSpecification<T> specs)
        {
            try
            {
                var query = AddSpecQueryAsync(specs);
                var entity = await query.FirstOrDefaultAsync();
                if (entity == null)
                {
                    return Result<T>.Failure($"{typeof(T).Name} matching specification not found.");
                }
                return Result<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error retrieving first entity by specification: {ex.Message}");
            }
        }

        public async Task<Result<T?>> GetFirstOrDefaultBySpecAsync(BaseSpecification<T> specs)
        {
            try
            {
                var query = AddSpecQueryAsync(specs);
                var entity = await query.FirstOrDefaultAsync();
                return Result<T?>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T?>.Failure($"Error retrieving first or default entity by specification: {ex.Message}");
            }
        }

        public async Task<Result> RemoveAsync(T entity)
        {
            try
            {
                Entities.Remove(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error removing entity: {ex.Message}");
            }
        }

        public async Task<Result> RemoveByIdAsync(object id)
        {
            try
            {
                var entity = await Entities.FindAsync(id);
                if (entity == null)
                {
                    return Result.Failure($"{typeof(T).Name} with ID {id} not found for removal.");
                }
                Entities.Remove(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error removing entity by ID: {ex.Message}");
            }
        }

        public async Task<Result> RemoveBySpecAsync(BaseSpecification<T> specification)
        {
            try
            {
                var entitiesToRemove = await AddSpecQueryAsync(specification).ToListAsync();
                if (!entitiesToRemove.Any())
                {
                    return Result.Failure("No entities found matching the specification for removal.");
                }
                Entities.RemoveRange(entitiesToRemove);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error removing entities by specification: {ex.Message}");
            }
        }


        public async Task<Result> UpdateRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                Entities.UpdateRange(entities);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating range of entities: {ex.Message}");
            }
        }

        #region private methods
        private IQueryable<T> AddSpecQueryAsync(BaseSpecification<T> specs)
        {
            var query = Entities.AsQueryable();

            if (specs.Criteria != null)
                query = query.Where(specs.Criteria);

            return query;
        }

        async Task<Result<T>> IBaseRepository<T>.GetFirstOrDefaultBySpecAsync(BaseSpecification<T> specs)
        {
            try
            {
                var query = AddSpecQueryAsync(specs);
                var entity = await query.FirstOrDefaultAsync();
                return Result<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error retrieving first or default entity by specification: {ex.Message}");
            }
        }

        public async Task<Result> UpdateAsync(T entity)
        {
            try
            {
                Entities.Update(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating entity: {ex.Message}");
            }
        }

        #endregion
    }
}
