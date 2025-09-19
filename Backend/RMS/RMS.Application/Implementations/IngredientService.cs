using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.IngredientDTOs.InputDTOs;
using RMS.Application.DTOs.IngredientDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Infrastructure.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Extensions;
using System.Linq;

namespace RMS.Application.Implementations
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateIngredientDto> _createValidator;
        private readonly IValidator<UpdateIngredientDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IngredientService> _logger;

        public IngredientService(
            IIngredientRepository ingredientRepository,
            IMapper mapper,
            IValidator<CreateIngredientDto> createValidator,
            IValidator<UpdateIngredientDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<IngredientService> logger)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<IngredientDto>> GetByIdAsync(int id)
        {
            var query = _ingredientRepository.GetQueryable();
            var ingredient = await query.Include(i => i.Unit)
                                        .Include(i => i.Supplier)
                                        .FirstOrDefaultAsync(i => i.IngredientID == id);

            if (ingredient == null)
            {
                return new ResponseDto<IngredientDto> { IsSuccess = false, Message = "Ingredient not found.", Code = "404" };
            }
            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);
            return new ResponseDto<IngredientDto> { IsSuccess = true, Data = ingredientDto, Code = "200" };
        }

        public async Task<PagedResult<IngredientDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _ingredientRepository.GetQueryable();

            query = query.Include(i => i.Unit)
                         .Include(i => i.Supplier);

            if (status.HasValue)
            {
                query = query.Where(i => i.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(i => i.Name.Contains(searchQuery) || i.Remarks.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.ApplySort(sortColumn, sortDirection ?? "asc");
            }
            else
            {
                query = query.OrderBy(i => i.Name);
            }

            var pagedResult = await query.ToPagedList(pageNumber, pageSize);
            var ingredientDtos = _mapper.Map<List<IngredientDto>>(pagedResult.Items);
            return new PagedResult<IngredientDto>(ingredientDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }

        public async Task<ResponseDto<IngredientDto>> CreateAsync(CreateIngredientDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errorDetails = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                _logger.LogError("Ingredient creation validation failed. Details: {ErrorDetails}", System.Text.Json.JsonSerializer.Serialize(errorDetails));
                return new ResponseDto<IngredientDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = errorDetails };
            }

            var ingredient = _mapper.Map<Ingredient>(createDto);
            await _ingredientRepository.AddAsync(ingredient);
            await _unitOfWork.SaveChangesAsync();

            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);
            await _auditLogService.LogAsync("CreateIngredient", "Ingredient", $"IngredientId:{ingredient.IngredientID}", "System", $"Ingredient '{ingredient.Name}' created.");

            return new ResponseDto<IngredientDto> { IsSuccess = true, Message = "Ingredient created successfully.", Data = ingredientDto, Code = "201" };
        }

        public async Task<ResponseDto<IngredientDto>> UpdateAsync(UpdateIngredientDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<IngredientDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var existingIngredient = await _ingredientRepository.GetByIdAsync(updateDto.IngredientID);
            if (existingIngredient == null)
            {
                return new ResponseDto<IngredientDto> { IsSuccess = false, Message = "Ingredient not found.", Code = "404" };
            }

            _mapper.Map(updateDto, existingIngredient);
            await _ingredientRepository.UpdateAsync(existingIngredient);
            await _unitOfWork.SaveChangesAsync();

            var ingredientDto = _mapper.Map<IngredientDto>(existingIngredient);
            await _auditLogService.LogAsync("UpdateIngredient", "Ingredient", $"IngredientId:{existingIngredient.IngredientID}", "System", $"Ingredient '{existingIngredient.Name}' updated.");

            return new ResponseDto<IngredientDto> { IsSuccess = true, Message = "Ingredient updated successfully.", Data = ingredientDto, Code = "200" };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);
            if (ingredient == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Ingredient not found.", Code = "404" };
            }

            await _ingredientRepository.DeleteAsync(ingredient);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("DeleteIngredient", "Ingredient", $"IngredientId:{id}", "System", $"Ingredient '{ingredient.Name}' deleted.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Ingredient deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);
            if (ingredient == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Ingredient not found.", Code = "404" };
            }

            ingredient.Status = status;
            await _ingredientRepository.UpdateAsync(ingredient);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("UpdateIngredientStatus", "Ingredient", $"IngredientId:{id}", "System", $"Ingredient '{ingredient.Name}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Ingredient status updated successfully.", Code = "200" };
        }
    }
}
