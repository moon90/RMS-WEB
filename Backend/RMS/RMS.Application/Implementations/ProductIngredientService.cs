using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.ProductIngredientDTOs.InputDTOs;
using RMS.Application.DTOs.ProductIngredientDTOs.OutputDTOs;
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
    public class ProductIngredientService : IProductIngredientService
    {
        private readonly IProductIngredientRepository _productIngredientRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductIngredientDto> _createValidator;
        private readonly IValidator<UpdateProductIngredientDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductIngredientService> _logger;

        public ProductIngredientService(
            IProductIngredientRepository productIngredientRepository,
            IMapper mapper,
            IValidator<CreateProductIngredientDto> createValidator,
            IValidator<UpdateProductIngredientDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<ProductIngredientService> logger)
        {
            _productIngredientRepository = productIngredientRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<ProductIngredientDto>> GetByIdAsync(int id)
        {
            var query = _productIngredientRepository.GetQueryable();
            var productIngredient = await query.Include(pi => pi.Product)
                                               .Include(pi => pi.Ingredient)
                                               .Include(pi => pi.Unit)
                                               .FirstOrDefaultAsync(pi => pi.ProductIngredientID == id);

            if (productIngredient == null)
            {
                return new ResponseDto<ProductIngredientDto> { IsSuccess = false, Message = "Product Ingredient not found.", Code = "404" };
            }
            var productIngredientDto = _mapper.Map<ProductIngredientDto>(productIngredient);
            return new ResponseDto<ProductIngredientDto> { IsSuccess = true, Data = productIngredientDto, Code = "200" };
        }

        public async Task<PagedResult<ProductIngredientDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _productIngredientRepository.GetQueryable();

            query = query.Include(pi => pi.Product)
                         .Include(pi => pi.Ingredient)
                         .Include(pi => pi.Unit);

            if (status.HasValue)
            {
                query = query.Where(pi => pi.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(pi => pi.Product != null && pi.Product.ProductName.Contains(searchQuery) || pi.Ingredient != null && pi.Ingredient.Name.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                if (sortColumn.Equals("productName", StringComparison.OrdinalIgnoreCase))
                {
                    if (sortDirection?.ToLower() == "desc")
                    {
                        query = query.OrderByDescending(pi => pi.Product.ProductName);
                    }
                    else
                    {
                        query = query.OrderBy(pi => pi.Product.ProductName);
                    }
                }
                else
                {
                    query = query.ApplySort(sortColumn, sortDirection ?? "asc");
                }
            }
            else
            {
                query = query.OrderBy(pi => pi.Product.ProductName); // Default sort
            }

            var pagedResult = await _productIngredientRepository.GetPagedResultAsync(new PagedQuery { PageNumber = pageNumber, PageSize = pageSize }, null, false, query);
            var productIngredientDtos = _mapper.Map<List<ProductIngredientDto>>(pagedResult.Items);
            return new PagedResult<ProductIngredientDto>(productIngredientDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }

        public async Task<ResponseDto<ProductIngredientDto>> CreateAsync(CreateProductIngredientDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errorDetails = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                _logger.LogError("Product Ingredient creation validation failed. Details: {ErrorDetails}", System.Text.Json.JsonSerializer.Serialize(errorDetails));
                return new ResponseDto<ProductIngredientDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = errorDetails };
            }

            var productIngredient = _mapper.Map<ProductIngredient>(createDto);
            await _productIngredientRepository.AddAsync(productIngredient);
            await _unitOfWork.SaveChangesAsync();

            var productIngredientDto = _mapper.Map<ProductIngredientDto>(productIngredient);
            await _auditLogService.LogAsync("CreateProductIngredient", "ProductIngredient", $"ProductIngredientId:{productIngredient.ProductIngredientID}", "System", $"Product Ingredient for product '{productIngredient.Product?.ProductName}' created.");

            return new ResponseDto<ProductIngredientDto> { IsSuccess = true, Message = "Product Ingredient created successfully.", Data = productIngredientDto, Code = "201" };
        }

        public async Task<ResponseDto<ProductIngredientDto>> UpdateAsync(UpdateProductIngredientDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<ProductIngredientDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var existingProductIngredient = await _productIngredientRepository.GetByIdAsync(updateDto.ProductIngredientID);
            if (existingProductIngredient == null)
            {
                return new ResponseDto<ProductIngredientDto> { IsSuccess = false, Message = "Product Ingredient not found.", Code = "404" };
            }

            _mapper.Map(updateDto, existingProductIngredient);
            await _productIngredientRepository.UpdateAsync(existingProductIngredient);
            await _unitOfWork.SaveChangesAsync();

            var productIngredientDto = _mapper.Map<ProductIngredientDto>(existingProductIngredient);
            await _auditLogService.LogAsync("UpdateProductIngredient", "ProductIngredient", $"ProductIngredientId:{existingProductIngredient.ProductIngredientID}", "System", $"Product Ingredient for product '{existingProductIngredient.Product?.ProductName}' updated.");

            return new ResponseDto<ProductIngredientDto> { IsSuccess = true, Message = "Product Ingredient updated successfully.", Data = productIngredientDto, Code = "200" };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var productIngredient = await _productIngredientRepository.GetByIdAsync(id);
            if (productIngredient == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Product Ingredient not found.", Code = "404" };
            }

            await _productIngredientRepository.DeleteAsync(productIngredient);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("DeleteProductIngredient", "ProductIngredient", $"ProductIngredientId:{id}", "System", $"Product Ingredient for product '{productIngredient.Product?.ProductName}' deleted.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Product Ingredient deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var productIngredient = await _productIngredientRepository.GetByIdAsync(id);
            if (productIngredient == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Product Ingredient not found.", Code = "404" };
            }

            productIngredient.Status = status;
            await _productIngredientRepository.UpdateAsync(productIngredient);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("UpdateProductIngredientStatus", "ProductIngredient", $"ProductIngredientId:{id}", "System", $"Product Ingredient for product '{productIngredient.Product?.ProductName}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Product Ingredient status updated successfully.", Code = "200" };
        }
    }
}
