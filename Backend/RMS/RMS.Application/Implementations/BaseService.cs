using AutoMapper;
using Microsoft.Extensions.Logging;
using RMS.Application.Interfaces;
using RMS.Core.Enum;
using RMS.Core.Exceptions;
using RMS.Core.Extensions;
using RMS.Core.Guard;
using RMS.Domain.Dtos.Dropdowns;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Domain.Specification;
using RMS.Infrastructure.IRepositories;
using System.Linq.Expressions;

namespace RMS.Application.Implementations
{
    public class BaseService<TEntity>(IMapper mapper,
                       IBaseRepository<TEntity> baseRepository,
                       IUnitOfWork unitOfWork,
                       ILogger<TEntity> logger) : IBaseService<TEntity> where TEntity : class
    {
        protected readonly IMapper Mapper = mapper;
        protected readonly IBaseRepository<TEntity> BaseRepository = baseRepository;
        protected readonly IUnitOfWork UnitOfWork = unitOfWork;
        protected readonly ILogger<TEntity> Logger = logger;

        #region Get methods
        public virtual async Task<Result<TDto>> GetByIdAsync<TDto>(object id) where TDto : class
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(GetByIdAsync)}: Fetching {typeof(TEntity).Name} with id {id}");

                GuardClause.Against.Null(id, nameof(id));

                if (id is int) IntGuardClause.Against.ZeroOrNegative(id, nameof(id));

                var entity = await BaseRepository.GetByIdAsync(id);

                return Result<TDto>.Success(Mapper.Map<TDto>(entity));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(BaseService<TEntity>)}:{nameof(GetByIdAsync)}: {ex.Message}");
                return Result<TDto>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<IEnumerable<DropdownDto>>> GetDropdownAsync(OrderSpecification<TEntity> spec)
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(GetDropdownAsync)}: Building drop down for {typeof(TEntity).Name}");
                var entities = await BaseRepository.GetOrderedAsync(spec);

                if (entities is null || !entities.Any())
                    return Result<IEnumerable<DropdownDto>>.Success([]);

                return Result<IEnumerable<DropdownDto>>.Success(Mapper.Map<IEnumerable<DropdownDto>>(entities));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(BaseService<TEntity>)}:{nameof(GetDropdownAsync)}: {ex.Message}");
                return Result<IEnumerable<DropdownDto>>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<IEnumerable<DropdownDto>>> GetDropdownAsync(BaseSpecification<TEntity> spec)
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(GetDropdownAsync)}: Building drop down for {typeof(TEntity).Name}");

                if (spec.Criteria is null)
                    return Result<IEnumerable<DropdownDto>>.Success([]);

                var entities = await BaseRepository.GetOrderedAsync(spec);

                if (entities is null || !entities.Any())
                    return Result<IEnumerable<DropdownDto>>.Success([]);

                return Result<IEnumerable<DropdownDto>>.Success(Mapper.Map<IEnumerable<DropdownDto>>(entities));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(BaseService<TEntity>)}:{nameof(GetDropdownAsync)}: {ex.Message}");
                return Result<IEnumerable<DropdownDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagedResult<TDto>>> GetPagedResultAsync<TDto>(PagedQuery query)
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(GetPagedResultAsync)}: Paging {typeof(TEntity).Name} with query: {query}");

                var entities = await BaseRepository.GetPagedResultAsync(query, BaseRepository.GetQueryable());

                return Result<PagedResult<TDto>>.Success(Mapper.Map<PagedResult<TDto>>(entities));
            }
            catch (Exception ex)
            {
                return Result<PagedResult<TDto>>.Failure(ex.Message);
            }
        }
        #endregion

        #region Add & Update Methods
        public virtual async Task<Result<TDto>> AddAsync<TDto>(TDto dto) where TDto : class
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(AddAsync)}: Adding {typeof(TEntity).Name}");
                var entity = Mapper.Map<TEntity>(dto);

                await BaseRepository.AddAsync(entity);

                var saved = await UnitOfWork.SaveChangesAsync();

                if (saved == 0)
                    return Result<TDto>.Failure($"Error adding the {typeof(TEntity).Name}", ResultType.ServerError);

                dto = Mapper.Map<TDto>(entity);

                return Result<TDto>.Success(dto);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(BaseService<TEntity>)}:{nameof(AddAsync)}: {ex.Message}");
                return Result<TDto>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<TDesDto>> AddAsync<TDesDto, TSrcDto>(TSrcDto dto) where TSrcDto : class where TDesDto : class
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(AddAsync)}: Adding {typeof(TEntity).Name}");
                GuardClause.Against.Null(dto, typeof(TSrcDto).Name);

                var entity = Mapper.Map<TEntity>(dto);

                await BaseRepository.AddAsync(entity);

                var saved = await UnitOfWork.SaveChangesAsync();

                if (saved == 0)
                    return Result<TDesDto>.Failure($"Error adding the {typeof(TEntity).Name}", ResultType.ServerError);

                var result = Mapper.Map<TDesDto>(entity);

                return Result<TDesDto>.Success(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(BaseService<TEntity>)}:{nameof(AddAsync)}: {ex.Message}");
                return Result<TDesDto>.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateAsync<TDto>(TDto dto, object id, Expression<Func<TEntity, bool>> condition) where TDto : class
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(UpdateAsync)}: Updating {typeof(TEntity).Name} with id: {id}");

                GuardClause.Against.Null(dto, typeof(TDto).Name);
                GuardClause.Against.Null(id, nameof(id));

                if (id is int) IntGuardClause.Against.ZeroOrNegative(id, nameof(id));

                if (await BaseRepository.ExistsAsync(condition))
                    return Result.Failure("Something need unique", ResultType.Exist);

                var entity = await BaseRepository.GetByIdAsync(id);

                Mapper.Map(dto, entity);

                return await UnitOfWork.SaveChangesAsync() == 0
                    ? Result<TDto>.Failure($"Error adding the {typeof(TEntity).Name}", ResultType.ServerError)
                    : Result.Success();
            }
            catch (InvalidInputException ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(UpdateAsync)}: Input not valid - detail: {ex.Message}");
                return Result.Failure(ex.Message, ResultType.InvalidInput);
            }
            catch (NotFoundException ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(UpdateAsync)}: Not found the {typeof(TEntity).Name} - detail: {ex.Message}");
                return Result.Failure(ex.Message, ResultType.NotFound);
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(UpdateAsync)}: Something went wrong {typeof(TEntity).Name} - detail: {ex.Message}");
                return Result.Failure(ex.Message, ResultType.ServerError);
            }
        }
        #endregion

        #region implemetation for Soft and Hard DELETE methods
        public virtual async Task<Result> SoftDeleteAsync(object id)
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(SoftDeleteAsync)}: Soft deleting {typeof(TEntity).Name} with id {id}");

                GuardClause.Against.Null(id, nameof(id));

                if (id is int) IntGuardClause.Against.ZeroOrNegative(id, nameof(id));

                var entity = await BaseRepository.GetByIdAsync(id);

                if (entity is ISoftDelete softDeletableEntity)
                {
                    softDeletableEntity.IsDeleted = true;
                    return await UnitOfWork.SaveChangesAsync() == 0 ? Result.Failure($"Error deleting the {typeof(TEntity).Name}", ResultType.ServerError) : Result.Success();
                }

                return Result.Failure($"Soft delete is not supported for {typeof(TEntity).Name}", ResultType.ServerError);
            }
            catch (InvalidInputException ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(SoftDeleteAsync)}: Input not valid - detail: {ex.Message}");
                return Result.Failure(ex.Message, ResultType.InvalidInput);
            }
            catch (NotFoundException ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(SoftDeleteAsync)}: Not found the {typeof(TEntity).Name} - detail: {ex.Message}");
                return Result.Failure(ex.Message, ResultType.NotFound);
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(SoftDeleteAsync)}: Something went wrong - detail: {ex.Message}");
                return Result.Failure(ex.Message, ResultType.ServerError);
            }
        }
        #endregion

        public virtual async Task<Result<IEnumerable<TDto>>> GetBySpecAsync<TDto>(BaseSpecification<TEntity> specs)
        {
            try
            {
                if (specs.IsNull())
                    return Result<IEnumerable<TDto>>.Failure(MessageExtensions.CouldNotBeNull(nameof(BaseSpecification<TEntity>)));

                var entity = await BaseRepository.GetBySpecAsync(specs);

                var result = Mapper.Map<IEnumerable<TDto>>(entity.ToList());

                return Result<IEnumerable<TDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TDto>>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<IQueryable<TEntity>>> GetBySpecAsync(BaseSpecification<TEntity> specs)
        {
            try
            {
                if (specs.IsNull())
                    return Result<IQueryable<TEntity>>.Failure(MessageExtensions.CouldNotBeNull(nameof(BaseSpecification<TEntity>)));

                var queries = await BaseRepository.GetBySpecAsync(specs);

                return Result<IQueryable<TEntity>>.Success(queries.AsQueryable());
            }
            catch (Exception ex)
            {
                return Result<IQueryable<TEntity>>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<TDto>> GetFisrtOrDefaultBySpecAsync<TDto>(BaseSpecification<TEntity> specs) where TDto : class
        {
            try
            {
                if (specs.IsNull())
                    return Result<TDto>.Failure(MessageExtensions.CouldNotBeNull(nameof(BaseSpecification<TEntity>)));

                var result = await BaseRepository.GetFirstOrDefaultBySpecAsync(specs);

                if (!result.Succeeded)
                    return Result<TDto>.Failure(result.Error);

                var dto = Mapper.Map<TDto>(result.Data);

                return Result<TDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<TDto>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<TEntity>> GetFisrtOrDefaultBySpecAsync(BaseSpecification<TEntity> specs)
        {
            try
            {
                if (specs.IsNull())
                    return Result<TEntity>.Failure(MessageExtensions.CouldNotBeNull(nameof(BaseSpecification<TEntity>)));

                return await BaseRepository.GetFirstOrDefaultBySpecAsync(specs);
            }
            catch (Exception ex)
            {
                return Result<TEntity>.Failure(ex.Message);
            }
        }


        public async Task<Result> AddRangeAsync<TDto>(IEnumerable<TDto> dtos)
        {
            try
            {
                if (dtos.IsNull() || !dtos.Any())
                    return Result.Failure(MessageExtensions.CouldNotBeNullOrEmpty(typeof(TDto).Name));

                var entities = Mapper.Map<IEnumerable<TEntity>>(dtos);

                return await BaseRepository.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }


        public async Task<Result> UpdateRangeAsync<TDto>(IEnumerable<TDto> dtos)
        {
            try
            {
                if (dtos.IsNull() || !dtos.Any())
                    return Result.Failure(MessageExtensions.CouldNotBeNullOrEmpty(typeof(TDto).Name));

                var entities = Mapper.Map<IEnumerable<TEntity>>(dtos);

                return await BaseRepository.UpdateRangeAsync(entities);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        #region Delete and remove methods
        public async Task<Result> RemoveBySpecAsync(BaseSpecification<TEntity> specification)
        {
            try
            {
                if (specification.IsNull())
                    return Result.Failure(MessageExtensions.CouldNotBeNull(typeof(BaseSpecification<TEntity>).Name));

                return await BaseRepository.RemoveBySpecAsync(specification);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> RemoveByIdAsync(object id)
        {
            try
            {
                if (id.IsNull())
                    return Result.Failure(MessageExtensions.CouldNotBeNull(nameof(id)));

                return await BaseRepository.RemoveByIdAsync(id);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteByIdAsync(object id)
        {
            try
            {
                if (id.IsNull())
                    return Result.Failure(MessageExtensions.CouldNotBeNull(nameof(id)));

                return await BaseRepository.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result<TEntity>> GetByIdAsync(object id)
        {
            var entity = await BaseRepository.GetByIdAsync(id);
            return Result<TEntity>.Failure("dddd");
        }
        #endregion
    }
}
