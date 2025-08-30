
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.ProductDTOs.InputDTOs;
using RMS.Application.DTOs.ProductDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Infrastructure.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper,
            IValidator<CreateProductDto> createValidator,
            IValidator<UpdateProductDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(int id)
        {
            var productResult = await _productRepository.GetByIdAsync(id);
            if (!productResult.Succeeded || productResult.Data == null)
            {
                return new ResponseDto<ProductDto> { IsSuccess = false, Message = "Product not found.", Code = "404" };
            }
            var productDto = _mapper.Map<ProductDto>(productResult.Data);
            return new ResponseDto<ProductDto> { IsSuccess = true, Data = productDto, Code = "200" };
        }

        public async Task<PagedResult<ProductDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _productRepository.GetQueryable();

            query = query.Include(p => p.Category)
                         .Include(p => p.Supplier)
                         .Include(p => p.Manufacturer);

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.ProductName.Contains(searchQuery) || p.ProductBarcode.Contains(searchQuery));
            }

            var pagedResult = await _productRepository.GetPagedResultAsync(new PagedQuery { PageNumber = pageNumber, PageSize = pageSize }, null, false, query);
            var productDtos = _mapper.Map<List<ProductDto>>(pagedResult.Items);
            return new PagedResult<ProductDto>(productDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }

        public async Task<ResponseDto<ProductDto>> CreateAsync(CreateProductDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errorDetails = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                _logger.LogError("Product creation validation failed. Details: {ErrorDetails}", System.Text.Json.JsonSerializer.Serialize(errorDetails));
                return new ResponseDto<ProductDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = errorDetails };
            }

            var product = _mapper.Map<Product>(createDto);
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);
            await _auditLogService.LogAsync("CreateProduct", "Product", $"ProductId:{product.Id}", "System", $"Product '{product.ProductName}' created.");

            return new ResponseDto<ProductDto> { IsSuccess = true, Message = "Product created successfully.", Data = productDto, Code = "201" };
        }

        public async Task<ResponseDto<ProductDto>> UpdateAsync(UpdateProductDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<ProductDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var existingProductResult = await _productRepository.GetByIdAsync(updateDto.Id);
            if (!existingProductResult.Succeeded || existingProductResult.Data == null)
            {
                return new ResponseDto<ProductDto> { IsSuccess = false, Message = "Product not found.", Code = "404" };
            }

            var existingProduct = existingProductResult.Data;
            _mapper.Map(updateDto, existingProduct);
            var updateResult = await _productRepository.UpdateAsync(existingProduct);
            if (!updateResult.Succeeded)
            {
                return new ResponseDto<ProductDto>
                {
                    IsSuccess = false,
                    Message = updateResult.Error,
                    Code = "500"
                };
            }
            await _unitOfWork.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(existingProduct);
            await _auditLogService.LogAsync("UpdateProduct", "Product", $"ProductId:{existingProduct.Id}", "System", $"Product '{existingProduct.ProductName}' updated.");

            return new ResponseDto<ProductDto> { IsSuccess = true, Message = "Product updated successfully.", Data = productDto, Code = "200" };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var productResult = await _productRepository.GetByIdAsync(id);
            if (!productResult.Succeeded || productResult.Data == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Product not found.", Code = "404" };
            }
            var product = productResult.Data;

            var deleteResult = await _productRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();

            if (!deleteResult.Succeeded)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = deleteResult.Error,
                    Code = "500"
                };
            }

            await _auditLogService.LogAsync("DeleteProduct", "Product", $"ProductId:{id}", "System", $"Product '{product.ProductName}' deleted.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Product deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var productResult = await _productRepository.GetByIdAsync(id);
            if (!productResult.Succeeded || productResult.Data == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Product not found.", Code = "404" };
            }
            var product = productResult.Data;

            product.Status = status;
            var updateResult = await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            if (!updateResult.Succeeded)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = updateResult.Error,
                    Code = "500"
                };
            }

            await _auditLogService.LogAsync("UpdateProductStatus", "Product", $"ProductId:{id}", "System", $"Product '{product.ProductName}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Product status updated successfully.", Code = "200" };
        }
    }
}
