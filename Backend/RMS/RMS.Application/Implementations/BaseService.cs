using AutoMapper;
using Microsoft.Extensions.Logging;
using RMS.Application.Interfaces;
using RMS.Core.Enum;
using RMS.Core.Exceptions;
using RMS.Core.Extensions;
using RMS.Core.Guard;
using RMS.Application.DTOs;
using RMS.Application.DTOs.Dropdowns;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Domain.Specification;
using RMS.Infrastructure.IRepositories;
using System.Linq.Expressions;
using RMS.Domain.Extensions;

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
        public virtual async Task<ResponseDto<TDto>> GetByIdAsync<TDto>(object id) where TDto : class
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(GetByIdAsync)}: Fetching {typeof(TEntity).Name} with id {id}");

                GuardClause.Against.Null(id, nameof(id));

                if (id is int) IntGuardClause.Against.ZeroOrNegative(id, nameof(id));

                var entity = await BaseRepository.GetByIdAsync(id);

                if (entity == null)
                {
                    return new ResponseDto<TDto>
                    {
                        IsSuccess = false,
                        Message = $"{typeof(TEntity).Name} not found.",
                        Code = "404"
                    };
                }

                return new ResponseDto<TDto>
                {
                    IsSuccess = true,
                    Message = $"{typeof(TEntity).Name} retrieved successfully.",
                    Code = "200",
                    Data = Mapper.Map<TDto>(entity)
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(BaseService<TEntity>)}:{nameof(GetByIdAsync)}: {ex.Message}");
                return new ResponseDto<TDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the entity.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public virtual async Task<ResponseDto<IEnumerable<DropdownDto>>> GetDropdownAsync(BaseSpecification<TEntity> spec)
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(GetDropdownAsync)}: Building drop down for {typeof(TEntity).Name}");

                if (spec == null)
                {
                    return new ResponseDto<IEnumerable<DropdownDto>>
                    {
                        IsSuccess = false,
                        Message = "Specification cannot be null.",
                        Code = "400"
                    };
                }

                var entities = await BaseRepository.GetOrderedAsync(spec);

                if (entities == null || !entities.Any())
                {
                    return new ResponseDto<IEnumerable<DropdownDto>>
                    {
                        IsSuccess = true,
                        Message = "No items found for dropdown.",
                        Code = "204",
                        Data = new List<DropdownDto>()
                    };
                }

                return new ResponseDto<IEnumerable<DropdownDto>>
                {
                    IsSuccess = true,
                    Message = "Dropdown items retrieved successfully.",
                    Code = "200",
                    Data = Mapper.Map<IEnumerable<DropdownDto>>(entities)
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(BaseService<TEntity>)}:{nameof(GetDropdownAsync)}: {ex.Message}");
                return new ResponseDto<IEnumerable<DropdownDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving dropdown items.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<PagedResult<TDto>>> GetPagedResultAsync<TDto>(PagedQuery query)
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(GetPagedResultAsync)}: Paging {typeof(TEntity).Name} with query: {query}");

                if (query == null)
                {
                    return new ResponseDto<PagedResult<TDto>>
                    {
                        IsSuccess = false,
                        Message = "Query cannot be null.",
                        Code = "400"
                    };
                }

                var queryable = BaseRepository.GetQueryable();

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    queryable = queryable.ApplySort(query.OrderBy, query.IsDescending ? "desc" : "asc");
                }

                var entities = await BaseRepository.GetPagedResultAsync(query, queryableInput: queryable);

                return new ResponseDto<PagedResult<TDto>>
                {
                    IsSuccess = true,
                    Message = "Paged results retrieved successfully.",
                    Code = "200",
                    Data = Mapper.Map<PagedResult<TDto>>(entities)
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(BaseService<TEntity>)}:{nameof(GetPagedResultAsync)}: {ex.Message}");
                return new ResponseDto<PagedResult<TDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving paged results.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }
        #endregion

        #region Add & Update Methods
        public virtual async Task<ResponseDto<TDto>> AddAsync<TDto>(TDto dto) where TDto : class
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(AddAsync)}: Adding {typeof(TEntity).Name}");
                GuardClause.Against.Null(dto, typeof(TDto).Name);

                var entity = Mapper.Map<TEntity>(dto);

                await BaseRepository.AddAsync(entity);

                var saved = await UnitOfWork.SaveChangesAsync();

                if (saved == 0)
                {
                    return new ResponseDto<TDto>
                    {
                        IsSuccess = false,
                        Message = $"Error adding the {typeof(TEntity).Name}.",
                        Code = "500"
                    };
                }

                return new ResponseDto<TDto>
                {
                    IsSuccess = true,
                    Message = $"{typeof(TEntity).Name} added successfully.",
                    Code = "201",
                    Data = Mapper.Map<TDto>(entity)
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(BaseService<TEntity>)}:{nameof(AddAsync)}: {ex.Message}");
                return new ResponseDto<TDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the entity.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public virtual async Task<ResponseDto<TDesDto>> AddAsync<TDesDto, TSrcDto>(TSrcDto dto) where TSrcDto : class where TDesDto : class
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(AddAsync)}: Adding {typeof(TEntity).Name}");
                GuardClause.Against.Null(dto, typeof(TSrcDto).Name);

                var entity = Mapper.Map<TEntity>(dto);

                await BaseRepository.AddAsync(entity);

                var saved = await UnitOfWork.SaveChangesAsync();

                if (saved == 0)
                {
                    return new ResponseDto<TDesDto>
                    {
                        IsSuccess = false,
                        Message = $"Error adding the {typeof(TEntity).Name}.",
                        Code = "500"
                    };
                }

                return new ResponseDto<TDesDto>
                {
                    IsSuccess = true,
                    Message = $"{typeof(TEntity).Name} added successfully.",
                    Code = "201",
                    Data = Mapper.Map<TDesDto>(entity)
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(BaseService<TEntity>)}:{nameof(AddAsync)}: {ex.Message}");
                return new ResponseDto<TDesDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the entity.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<object>> UpdateAsync<TDto>(TDto dto, object id, Expression<Func<TEntity, bool>> condition) where TDto : class
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(UpdateAsync)}: Updating {typeof(TEntity).Name} with id: {id}");

                GuardClause.Against.Null(dto, typeof(TDto).Name);
                GuardClause.Against.Null(id, nameof(id));

                if (id is int) IntGuardClause.Against.ZeroOrNegative(id, nameof(id));

                if (await BaseRepository.ExistsAsync(condition))
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Something needs to be unique.",
                        Code = "409" // Conflict
                    };
                }

                var entity = await BaseRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = $"{typeof(TEntity).Name} not found.",
                        Code = "404"
                    };
                }

                Mapper.Map(dto, entity);

                var saved = await UnitOfWork.SaveChangesAsync();

                if (saved == 0)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = $"Error updating the {typeof(TEntity).Name}.",
                        Code = "500"
                    };
                }

                return new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = $"{typeof(TEntity).Name} updated successfully.",
                    Code = "200"
                };
            }
            catch (InvalidInputException ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(UpdateAsync)}: Input not valid - detail: {ex.Message}");
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Code = "400",
                    Details = ex.Message
                };
            }
            catch (NotFoundException ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(UpdateAsync)}: Not found the {typeof(TEntity).Name} - detail: {ex.Message}");
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Code = "404",
                    Details = ex.Message
                };
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(UpdateAsync)}: Something went wrong {typeof(TEntity).Name} - detail: {ex.Message}");
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the entity.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }
        #endregion

        #region implemetation for Soft and Hard DELETE methods
        public virtual async Task<ResponseDto<object>> SoftDeleteAsync(object id)
        {
            try
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(SoftDeleteAsync)}: Soft deleting {typeof(TEntity).Name} with id {id}");

                GuardClause.Against.Null(id, nameof(id));

                if (id is int) IntGuardClause.Against.ZeroOrNegative(id, nameof(id));

                var entity = await BaseRepository.GetByIdAsync(id);

                if (entity == null)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = $"{typeof(TEntity).Name} not found.",
                        Code = "404"
                    };
                }

                if (entity is ISoftDelete softDeletableEntity)
                {
                    softDeletableEntity.IsDeleted = true;
                    var saved = await UnitOfWork.SaveChangesAsync();
                    if (saved == 0)
                    {
                        return new ResponseDto<object>
                        {
                            IsSuccess = false,
                            Message = $"Error soft deleting the {typeof(TEntity).Name}.",
                            Code = "500"
                        };
                    }
                    return new ResponseDto<object>
                    {
                        IsSuccess = true,
                        Message = $"{typeof(TEntity).Name} soft deleted successfully.",
                        Code = "200"
                    };
                }

                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = $"Soft delete is not supported for {typeof(TEntity).Name}.",
                    Code = "400"
                };
            }
            catch (InvalidInputException ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(SoftDeleteAsync)}: Input not valid - detail: {ex.Message}");
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Code = "400",
                    Details = ex.Message
                };
            }
            catch (NotFoundException ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(SoftDeleteAsync)}: Not found the {typeof(TEntity).Name} - detail: {ex.Message}");
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Code = "404",
                    Details = ex.Message
                };
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"{nameof(BaseService<TEntity>)}:{nameof(SoftDeleteAsync)}: Something went wrong - detail: {ex.Message}");
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while soft deleting the entity.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }
        #endregion

        public virtual async Task<ResponseDto<IEnumerable<TDto>>> GetBySpecAsync<TDto>(BaseSpecification<TEntity> specs)
        {
            try
            {
                if (specs == null)
                {
                    return new ResponseDto<IEnumerable<TDto>>
                    {
                        IsSuccess = false,
                        Message = "Specification cannot be null.",
                        Code = "400"
                    };
                }

                var entity = await BaseRepository.GetBySpecAsync(specs);

                return new ResponseDto<IEnumerable<TDto>>
                {
                    IsSuccess = true,
                    Message = "Entities retrieved by specification successfully.",
                    Code = "200",
                    Data = Mapper.Map<IEnumerable<TDto>>(entity.ToList())
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<TDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving entities by specification.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public virtual async Task<ResponseDto<IQueryable<TEntity>>> GetBySpecAsync(BaseSpecification<TEntity> specs)
        {
            try
            {
                if (specs == null)
                {
                    return new ResponseDto<IQueryable<TEntity>>
                    {
                        IsSuccess = false,
                        Message = "Specification cannot be null.",
                        Code = "400"
                    };
                }

                var queries = await BaseRepository.GetBySpecAsync(specs);

                return new ResponseDto<IQueryable<TEntity>>
                {
                    IsSuccess = true,
                    Message = "Entities retrieved by specification successfully.",
                    Code = "200",
                    Data = queries.AsQueryable()
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IQueryable<TEntity>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving entities by specification.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public virtual async Task<ResponseDto<TDto>> GetFirstOrDefaultBySpecAsync<TDto>(BaseSpecification<TEntity> specs) where TDto : class
        {
            try
            {
                if (specs == null)
                {
                    return new ResponseDto<TDto>
                    {
                        IsSuccess = false,
                        Message = "Specification cannot be null.",
                        Code = "400"
                    };
                }

                var entities = await BaseRepository.GetBySpecAsync(specs);
                var entity = entities.FirstOrDefault();

                if (entity == null)
                {
                    return new ResponseDto<TDto>
                    {
                        IsSuccess = false,
                        Message = $"{typeof(TEntity).Name} not found.",
                        Code = "404" // Assuming not found if not succeeded
                    };
                }

                var dto = Mapper.Map<TDto>(entity);

                return new ResponseDto<TDto>
                {
                    IsSuccess = true,
                    Message = "Entity retrieved by specification successfully.",
                    Code = "200",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<TDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the entity by specification.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public virtual async Task<ResponseDto<TEntity>> GetFirstOrDefaultBySpecAsync(BaseSpecification<TEntity> specs)
        {
            try
            {
                if (specs == null)
                {
                    return new ResponseDto<TEntity>
                    {
                        IsSuccess = false,
                        Message = "Specification cannot be null.",
                        Code = "400"
                    };
                }

                var entities = await BaseRepository.GetBySpecAsync(specs);
                var entity = entities.FirstOrDefault();

                if (entity == null)
                {
                    return new ResponseDto<TEntity>
                    {
                        IsSuccess = false,
                        Message = $"{typeof(TEntity).Name} not found.",
                        Code = "404"
                    };
                }

                return new ResponseDto<TEntity>
                {
                    IsSuccess = true,
                    Message = "Entity retrieved by specification successfully.",
                    Code = "200",
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<TEntity>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the entity by specification.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        public async Task<ResponseDto<object>> AddRangeAsync<TDto>(IEnumerable<TDto> dtos)
        {
            try
            {
                if (dtos == null || !dtos.Any())
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "List of DTOs cannot be null or empty.",
                        Code = "400"
                    };
                }

                var entities = Mapper.Map<IEnumerable<TEntity>>(dtos);

                foreach (var entity in entities)
                {
                    await BaseRepository.AddAsync(entity);
                }

                var saved = await UnitOfWork.SaveChangesAsync();

                if (saved == 0)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Error adding entities.",
                        Code = "500"
                    };
                }

                return new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Entities added successfully.",
                    Code = "201"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding entities.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }


        public async Task<ResponseDto<object>> UpdateRangeAsync<TDto>(IEnumerable<TDto> dtos)
        {
            try
            {
                if (dtos == null || !dtos.Any())
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "List of DTOs cannot be null or empty.",
                        Code = "400"
                    };
                }

                var entities = Mapper.Map<IEnumerable<TEntity>>(dtos);

                foreach (var entity in entities)
                {
                    await BaseRepository.UpdateAsync(entity);
                }

                var saved = await UnitOfWork.SaveChangesAsync();

                if (saved == 0)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Error updating entities.",
                        Code = "500"
                    };
                }

                return new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Entities updated successfully.",
                    Code = "200"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating entities.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        #region Delete and remove methods
        public async Task<ResponseDto<object>> RemoveBySpecAsync(BaseSpecification<TEntity> specification)
        {
            try
            {
                if (specification == null)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Specification cannot be null.",
                        Code = "400"
                    };
                }

                var entitiesToDelete = await BaseRepository.GetBySpecAsync(specification);

                if (!entitiesToDelete.Any())
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "No entities found matching the specification for removal.",
                        Code = "404"
                    };
                }

                foreach (var entity in entitiesToDelete)
                {
                    await BaseRepository.DeleteAsync(entity);
                }

                var saved = await UnitOfWork.SaveChangesAsync();

                if (saved == 0)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Error removing entities by specification.",
                        Code = "500"
                    };
                }

                return new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Entities removed by specification successfully.",
                    Code = "200"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while removing entities by specification.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<object>> RemoveByIdAsync(object id)
        {
            try
            {
                if (id == null)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "ID cannot be null.",
                        Code = "400"
                    };
                }

                var entity = await BaseRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = $"{typeof(TEntity).Name} not found.",
                        Code = "404"
                    };
                }

                await BaseRepository.DeleteAsync(entity);
                var saved = await UnitOfWork.SaveChangesAsync();

                if (saved == 0)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = $"Error removing the {typeof(TEntity).Name}.",
                        Code = "500"
                    };
                }

                return new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Entity removed by ID successfully.",
                    Code = "200"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while removing the entity by ID.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<object>> DeleteByIdAsync(object id)
        {
            try
            {
                if (id == null)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "ID cannot be null.",
                        Code = "400"
                    };
                }

                var entity = await BaseRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = $"{typeof(TEntity).Name} not found.",
                        Code = "404"
                    };
                }

                await BaseRepository.DeleteAsync(entity);
                var saved = await UnitOfWork.SaveChangesAsync();

                if (saved == 0)
                {
                    return new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = $"Error deleting the {typeof(TEntity).Name}.",
                        Code = "500"
                    };
                }

                return new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Entity deleted by ID successfully.",
                    Code = "200"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the entity by ID.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<TEntity>> GetByIdAsync(object id)
        {
            try
            {
                var entity = await BaseRepository!.GetByIdAsync(id);

                if (entity == null)
                {
                    return new ResponseDto<TEntity>
                    {
                        IsSuccess = false,
                        Message = $"{typeof(TEntity).Name} not found.",
                        Code = "404", // Assuming not found if not succeeded
                    };
                }
                return new ResponseDto<TEntity>
                {
                    IsSuccess = true,
                    Message = $"{typeof(TEntity).Name} retrieved successfully.",
                    Code = "200",
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<TEntity>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the entity.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }
        #endregion
    }
}
