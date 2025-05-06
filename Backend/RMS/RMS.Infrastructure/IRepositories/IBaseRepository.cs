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
        #region GET methods
        /// <summary>
        /// Asynchronously retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the entity of type <typeparamref name="T"/> with the given identifier,
        /// or <c>null</c> if no entity is found with the provided id.
        /// </returns>
        Task<T> GetByIdAsync(object id);

        /// <summary>
        /// Asynchronously retrieves a paged result of entities based on the provided query parameters and an optional queryable input.
        /// </summary>
        /// <param name="param">The pagination parameters used to filter and paginate the results, such as page size and page number.</param>
        /// <param name="queryableInput">An optional <see cref="IQueryable{T}"/> that can be used to further filter 
        /// or modify the query before applying pagination. If <c>null</c>, the query will be executed on the entire data set.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a <see cref="PagedResult{T}"/> that includes:
        /// <list type="bullet">
        ///     <item><description>A list of entities of type <typeparamref name="T"/> for the requested page.</description></item>
        ///     <item><description>The total number of items available across all pages.</description></item>
        /// </list>
        /// </returns>
        /// <exception cref="NotFoundException">Thrown when the requested page number is invalid (e.g., 
        /// the page does not exist based on the total number of pages). 
        /// Example message: "Page {param.PageNumber} does not exist. Maximum page number is {totalPages}."</exception>
        Task<PagedResult<T>> GetPagedResultAsync(PagedQuery param, IQueryable<T>? queryableInput);

        /// <summary>
        /// Asynchronously retrieves a collection of entities ordered according to the specified order criteria.
        /// </summary>
        /// <param name="specs">The order specification that defines how the entities should be sorted.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> 
        /// collection of entities ordered based on the provided specification.
        /// </returns>
        Task<IEnumerable<T>> GetOrderedAsync(OrderSpecification<T> specs);

        /// <summary>
        /// Retrieves an <see cref="IQueryable{T}"/> that can be used to query entities of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> that represents the collection of entities of type <typeparamref name="T"/>.
        /// </returns>
        IQueryable<T> GetQueryable();

        /// <summary>
        /// Asynchronously retrieves a collection of entities that match the given specification.
        /// </summary>
        /// <param name="specs">The specification that defines the criteria for filtering the entities.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains an <see cref="IEnumerable{T}"/> collection of entities that match the provided specification.
        /// </returns>
        Task<IEnumerable<T>> GetBySpecAsync(BaseSpecification<T> specs);

        /// <summary>
        /// Asynchronously retrieves a collection of entities ordered according to the specified criteria in the given specification.
        /// </summary>
        /// <param name="specs">The specification that defines the criteria for filtering and ordering the entities.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains an <see cref="IEnumerable{T}"/> collection of entities ordered based on the provided specification.
        /// </returns>
        Task<IEnumerable<T>> GetOrderedAsync(BaseSpecification<T> specs);
        #endregion

        #region EXISTS methods
        /// <summary>
        /// Asynchronously checks if any entities exist that match the specified criteria.
        /// </summary>
        /// <param name="spec">The specification that defines the criteria for filtering the entities.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a <see cref="bool"/> value:
        /// <list type="bullet">
        ///     <item><description><c>true</c> if at least one entity matches the specified criteria.</description></item>
        ///     <item><description><c>false</c> if no entities match the specified criteria.</description></item>
        /// </list>
        /// </returns>
        Task<bool> ExistsAsync(BaseSpecification<T> spec);

        /// <summary>
        /// Asynchronously checks if any entities exist that match the given expression.
        /// </summary>
        /// <param name="expression">A lambda expression that defines the criteria for filtering the entities.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a <see cref="bool"/> value:
        /// <list type="bullet">
        ///     <item><description><c>true</c> if at least one entity matches the specified criteria defined by the expression.</description></item>
        ///     <item><description><c>false</c> if no entities match the specified criteria.</description></item>
        /// </list>
        /// </returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
        #endregion

        #region ADD methods
        /// <summary>
        /// Add the Entity
        /// </summary>
        /// <param name="entity">An Entity</param>
        /// <returns></returns>
        Task AddAsync(T entity);
        #endregion

        //TODO: implement when using
        Task<Result> AddRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllAsync();
        Task<Result<T>> GetFirstOrDefaultBySpecAsync(BaseSpecification<T> specs);
        Task<Result<T>> GetFirstBySpecAsync(BaseSpecification<T> specs);
        Task<Result> UpdateAsync(T entity);
        Task<Result> UpdateRangeAsync(IEnumerable<T> entities);
        Task<Result> DeleteByIdAsync(object id);
        Task<Result> RemoveByIdAsync(object id);
        Task<Result> RemoveAsync(T entity);
        Task<Result> RemoveBySpecAsync(BaseSpecification<T> specification);
    }
}
