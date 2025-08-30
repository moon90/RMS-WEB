using RMS.Domain.Dtos;
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
        Task<ResponseDto<TDto>> GetByIdAsync<TDto>(object id) where TDto : class;

        Task<ResponseDto<IEnumerable<DropdownDto>>> GetDropdownAsync(OrderSpecification<TEntity> spec);

        Task<ResponseDto<IEnumerable<DropdownDto>>> GetDropdownAsync(BaseSpecification<TEntity> spec);

        Task<ResponseDto<PagedResult<TDto>>> GetPagedResultAsync<TDto>(PagedQuery query);
        #endregion

        #region ADD methods
        Task<ResponseDto<TDesDto>> AddAsync<TDesDto, TSrcDto>(TSrcDto dto) where TSrcDto : class where TDesDto : class;

        Task<ResponseDto<TDto>> AddAsync<TDto>(TDto dto) where TDto : class;
        #endregion

        #region UPDATE methods
        Task<ResponseDto<object>> UpdateAsync<TDto>(TDto dto, object id, Expression<Func<TEntity, bool>> condition) where TDto : class;
        #endregion

        #region DELETE methods
        Task<ResponseDto<object>> SoftDeleteAsync(object id);
        #endregion


        // TODO: implement when using
        Task<ResponseDto<TDto>> GetFisrtOrDefaultBySpecAsync<TDto>(BaseSpecification<TEntity> specs) where TDto : class;
        Task<ResponseDto<TEntity>> GetFisrtOrDefaultBySpecAsync(BaseSpecification<TEntity> specs);
        Task<ResponseDto<IEnumerable<TDto>>> GetBySpecAsync<TDto>(BaseSpecification<TEntity> specs);
        Task<ResponseDto<IQueryable<TEntity>>> GetBySpecAsync(BaseSpecification<TEntity> specs);
        Task<ResponseDto<object>> AddRangeAsync<TDto>(IEnumerable<TDto> dtos);
        Task<ResponseDto<object>> UpdateRangeAsync<TDto>(IEnumerable<TDto> dtos);
        Task<ResponseDto<object>> DeleteByIdAsync(object id);
        Task<ResponseDto<object>> RemoveByIdAsync(object id);
        Task<ResponseDto<object>> RemoveBySpecAsync(BaseSpecification<TEntity> specification);
        Task<ResponseDto<TEntity>> GetByIdAsync(object id);
    }
}
