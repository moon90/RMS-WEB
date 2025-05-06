using RMS.Domain.Dtos.Dropdowns;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    /// <summary>
    /// Provides base service operations for entities of type <typeparamref name="TEntity"/>. 
    /// This class handles common service logic, including mapping, repository access, unit of work, and logging.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that the service operates on. It must be a class.</typeparam>
    /// <param name="mapper">An instance of <see cref="IMapper"/> to handle object mapping between entities and DTOs.</param>
    /// <param name="baseRepository">An instance of <see cref="IBaseRepository{TEntity}"/> to access the underlying data repository for CRUD operations.</param>
    /// <param name="unitOfWork">An instance of <see cref="IUnitOfWork"/> to manage transaction scope and commit changes to the data store.</param>
    /// <param name="logger">An instance of <see cref="ILogger{TEntity}"/> to log service operations and errors.</param>
    public interface IBaseService<TEntity> where TEntity : class
    {
        #region GET methods
        /// <summary>
        /// Get the Dto by <paramref name="id" />
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="id">The Id</param>
        /// <returns>The Result contain the TDto if it has been found; 
        /// Otherwise return falure with messange of error</returns>
        Task<Result<TDto>> GetByIdAsync<TDto>(object id) where TDto : class;

        /// <summary>
        /// Get the List of DropdowmDto item by OrderSpecification of Entity
        /// </summary>
        /// <param name="spec">OrderSpecification<TEntity></param>
        /// <returns>A List of DropdownDto</returns>
        Task<Result<IEnumerable<DropdownDto>>> GetDropdownAsync(OrderSpecification<TEntity> spec);

        /// <summary>
        /// Get the List of DropdowmDto item by BaseSpecification of Entity
        /// </summary>
        /// <param name="spec">BaseSpecification</param>
        /// <returns>A List of DropdownDto</returns>
        Task<Result<IEnumerable<DropdownDto>>> GetDropdownAsync(BaseSpecification<TEntity> spec);

        /// <summary>
        /// Retrieves a paged result of data based on the provided query parameters.
        /// </summary>
        /// <typeparam name="TDto">The type of the data transfer object (DTO) returned.</typeparam>
        /// <param name="query">The paging and filtering parameters for retrieving the data.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result contains a <see cref="Result{T}"/> wrapping a <see cref="PagedResult{TDto}"/> 
        /// with the requested data and pagination details.
        /// </returns>
        Task<Result<PagedResult<TDto>>> GetPagedResultAsync<TDto>(PagedQuery query);
        #endregion

        #region ADD methods
        /// <summary>
        /// Create new Entity
        /// </summary>
        /// <typeparam name="TDesDto">The Destination Dto</typeparam>
        /// <typeparam name="TSrcDto">The Source Dto</typeparam>
        /// <param name="dto">The Source Dto</param>
        /// <returns>The Result contain the Destination Dto</returns>
        Task<Result<TDesDto>> AddAsync<TDesDto, TSrcDto>(TSrcDto dto) where TSrcDto : class where TDesDto : class;

        /// <summary>
        /// Create new Entity
        /// </summary>
        /// <typeparam name="TDto">The Dto</typeparam>
        /// <param name="dto">The Source Dto</param>
        /// <returns>The Result contain the Dto</returns>
        /// Dev noted: Not test with real case yet!!!
        Task<Result<TDto>> AddAsync<TDto>(TDto dto) where TDto : class;
        #endregion

        #region UPDATE methods
        /// <summary>
        /// Update the Entity
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="dto">The Dto</param>
        /// <param name="id">The Id of Entity</param>
        /// <param name="condition">Property need be unique</param>
        /// <returns>The Result</returns>
        Task<Result> UpdateAsync<TDto>(TDto dto, object id, Expression<Func<TEntity, bool>> condition) where TDto : class;
        #endregion

        #region DELETE methods
        /// <summary>
        /// Soft delete the Entity by the Id
        /// </summary>
        /// <param name="id">The Id of the Company</param>
        /// <returns>The Result</returns>
        Task<Result> SoftDeleteAsync(object id);
        #endregion


        // TODO: implement when using
        Task<Result<TDto>> GetFisrtOrDefaultBySpecAsync<TDto>(BaseSpecification<TEntity> specs) where TDto : class;
        Task<Result<TEntity>> GetFisrtOrDefaultBySpecAsync(BaseSpecification<TEntity> specs);
        Task<Result<IEnumerable<TDto>>> GetBySpecAsync<TDto>(BaseSpecification<TEntity> specs);
        Task<Result<IQueryable<TEntity>>> GetBySpecAsync(BaseSpecification<TEntity> specs);
        Task<Result> AddRangeAsync<TDto>(IEnumerable<TDto> dtos);
        Task<Result> UpdateRangeAsync<TDto>(IEnumerable<TDto> dtos);
        Task<Result> DeleteByIdAsync(object id);
        Task<Result> RemoveByIdAsync(object id);
        Task<Result> RemoveBySpecAsync(BaseSpecification<TEntity> specification);
        Task<Result<TEntity>> GetByIdAsync(object id);
    }
}
