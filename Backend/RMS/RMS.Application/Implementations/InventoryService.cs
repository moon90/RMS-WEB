using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.InventoryDTOs.InputDTOs;
using RMS.Application.DTOs.InventoryDTOs.OutputDTOs;
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
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateInventoryDto> _createValidator;
        private readonly IValidator<UpdateInventoryDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(
            IInventoryRepository inventoryRepository,
            IMapper mapper,
            IValidator<CreateInventoryDto> createValidator,
            IValidator<UpdateInventoryDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<InventoryService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<InventoryDto>> GetByIdAsync(int id)
        {
            var query = _inventoryRepository.GetQueryable();
            var inventory = await query.Include(i => i.Product)
                                     .FirstOrDefaultAsync(i => i.InventoryID == id);

            if (inventory == null)
            {
                return new ResponseDto<InventoryDto> { IsSuccess = false, Message = "Inventory not found.", Code = "404" };
            }
            var inventoryDto = _mapper.Map<InventoryDto>(inventory);
            return new ResponseDto<InventoryDto> { IsSuccess = true, Data = inventoryDto, Code = "200" };
        }

        public async Task<PagedResult<InventoryDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _inventoryRepository.GetQueryable();

            query = query.Include(i => i.Product);

            if (status.HasValue)
            {
                query = query.Where(i => i.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(i => i.Product != null && i.Product.ProductName.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                if (sortColumn.Equals("productName", StringComparison.OrdinalIgnoreCase))
                {
                    if (sortDirection?.ToLower() == "desc")
                    {
                        query = query.OrderByDescending(i => i.Product.ProductName);
                    }
                    else
                    {
                        query = query.OrderBy(i => i.Product.ProductName);
                    }
                }
                else
                {
                    query = query.ApplySort(sortColumn, sortDirection ?? "asc");
                }
            }
            else
            {
                query = query.OrderBy(i => i.Product.ProductName);
            }

            var pagedResult = await query.ToPagedList(pageNumber, pageSize);
            var inventoryDtos = _mapper.Map<List<InventoryDto>>(pagedResult.Items);
            return new PagedResult<InventoryDto>(inventoryDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }

        public async Task<ResponseDto<InventoryDto>> CreateAsync(CreateInventoryDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errorDetails = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                _logger.LogError("Inventory creation validation failed. Details: {ErrorDetails}", System.Text.Json.JsonSerializer.Serialize(errorDetails));
                return new ResponseDto<InventoryDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = errorDetails };
            }

            var inventory = _mapper.Map<Inventory>(createDto);
            await _inventoryRepository.AddAsync(inventory);
            await _unitOfWork.SaveChangesAsync();

            var inventoryDto = _mapper.Map<InventoryDto>(inventory);
            await _auditLogService.LogAsync("CreateInventory", "Inventory", $"InventoryId:{inventory.InventoryID}", "System", $"Inventory for product '{inventory.Product?.ProductName}' created.");

            return new ResponseDto<InventoryDto> { IsSuccess = true, Message = "Inventory created successfully.", Data = inventoryDto, Code = "201" };
        }

        public async Task<ResponseDto<InventoryDto>> UpdateAsync(UpdateInventoryDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<InventoryDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var existingInventory = await _inventoryRepository.GetByIdAsync(updateDto.InventoryID);
            if (existingInventory == null)
            {
                return new ResponseDto<InventoryDto> { IsSuccess = false, Message = "Inventory not found.", Code = "404" };
            }

            _mapper.Map(updateDto, existingInventory);
            await _inventoryRepository.UpdateAsync(existingInventory);
            await _unitOfWork.SaveChangesAsync();

            var inventoryDto = _mapper.Map<InventoryDto>(existingInventory);
            await _auditLogService.LogAsync("UpdateInventory", "Inventory", $"InventoryId:{existingInventory.InventoryID}", "System", $"Inventory for product '{existingInventory.Product?.ProductName}' updated.");

            return new ResponseDto<InventoryDto> { IsSuccess = true, Message = "Inventory updated successfully.", Data = inventoryDto, Code = "200" };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(id);
            if (inventory == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Inventory not found.", Code = "404" };
            }

            await _inventoryRepository.DeleteAsync(inventory);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("DeleteInventory", "Inventory", $"InventoryId:{id}", "System", $"Inventory for product '{inventory.Product?.ProductName}' deleted.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Inventory deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(id);
            if (inventory == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Inventory not found.", Code = "404" };
            }

            inventory.Status = status;
            await _inventoryRepository.UpdateAsync(inventory);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("UpdateInventoryStatus", "Inventory", $"InventoryId:{id}", "System", $"Inventory for product '{inventory.Product?.ProductName}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Inventory status updated successfully.", Code = "200" };
        }

        public async Task<PagedResult<InventoryDto>> GetLowStockProductsAsync(int pageNumber, int pageSize)
        {
            var query = _inventoryRepository.GetQueryable()
                                                                .Include(i => i.Product)
                                                                .Where(i => i.CurrentStock < i.MinStockLevel);

            // Add sorting for low stock products
            query = query.OrderBy(i => i.Product.ProductName); // Default sort for low stock

            var pagedResult = await query.ToPagedList(pageNumber, pageSize);
            var inventoryDtos = _mapper.Map<List<InventoryDto>>(pagedResult.Items);
            return new PagedResult<InventoryDto>(inventoryDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }
    }
}
