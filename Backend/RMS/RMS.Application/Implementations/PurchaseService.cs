
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Core.Enum;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Domain.Specification;
using RMS.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Application.Implementations
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePurchaseDto> _createPurchaseValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<PurchaseService> _logger;
        private readonly IAlertService _alertService;

        public PurchaseService(
            IPurchaseRepository purchaseRepository,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreatePurchaseDto> createPurchaseValidator,
            IAuditLogService auditLogService,
            ILogger<PurchaseService> logger,
            IAlertService alertService)
        {
            _purchaseRepository = purchaseRepository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createPurchaseValidator = createPurchaseValidator;
            _auditLogService = auditLogService;
            _logger = logger;
            _alertService = alertService;
        }

        public async Task<ResponseDto<PurchaseDto>> CreatePurchaseAsync(CreatePurchaseDto purchaseDto)
        {
            var validationResult = await _createPurchaseValidator.ValidateAsync(purchaseDto);
            if (!validationResult.IsValid)
            {
                return ResponseDto<PurchaseDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var purchase = _mapper.Map<Purchase>(purchaseDto);
                var newPurchase = await _purchaseRepository.AddAsync(purchase);

                foreach (var detail in purchaseDto.PurchaseDetails)
                {
                    var product = await _productRepository.GetByIdAsync(detail.ProductId);
                    var inventory = await _inventoryRepository.GetByProductIdAsync(detail.ProductId);

                    if (product != null && inventory != null)
                    {
                        var oldQuantity = inventory.CurrentStock;
                        var oldCostPrice = product.CostPrice ?? 0;
                        var newQuantity = detail.Quantity;
                        var newPurchasePrice = detail.UnitPrice;

                        var newCostPrice = ((oldQuantity * oldCostPrice) + (newQuantity * newPurchasePrice)) / (oldQuantity + newQuantity);

                        product.CostPrice = newCostPrice;
                        await _productRepository.UpdateAsync(product);

                        inventory.CurrentStock += newQuantity;
                        await _inventoryRepository.UpdateAsync(inventory);

                        if (oldQuantity < inventory.MinStockLevel && inventory.CurrentStock >= inventory.MinStockLevel)
                        {
                            await _alertService.CreateAlertAsync(new DTOs.AlertDTOs.CreateAlertDto
                            {
                                Message = $"Stock for product {product.ProductName} has been replenished.",
                                Type = Domain.Enum.AlertType.LowStock
                            });
                        }
                    }
                    else
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ResponseDto<PurchaseDto>.CreateErrorResponse($"Product or Inventory not found for Product ID: {detail.ProductId}", ApiErrorCode.NotFound);
                    }
                }

                await _unitOfWork.CommitTransactionAsync();

                await _auditLogService.LogAsync("CreatePurchase", "Purchase", $"PurchaseId:{newPurchase.PurchaseID}", "System", $"Purchase created for supplier '{newPurchase.SupplierID}'.");

                return ResponseDto<PurchaseDto>.CreateSuccessResponse(_mapper.Map<PurchaseDto>(newPurchase), "Purchase created successfully.", "201");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "An error occurred while creating the purchase.");
                return ResponseDto<PurchaseDto>.CreateErrorResponse("An error occurred while creating the purchase.", ApiErrorCode.ServerError);
            }
        }

        public async Task<ResponseDto<List<PurchaseDto>>> GetAllPurchasesAsync()
        {
            var spec = new PurchaseWithCategorySpecification();
            var purchases = await _purchaseRepository.GetBySpecAsync(spec);
            return ResponseDto<List<PurchaseDto>>.CreateSuccessResponse(_mapper.Map<List<PurchaseDto>>(purchases));
        }

        public async Task<ResponseDto<PurchaseDto>> GetPurchaseByIdAsync(int id)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(id);
            if (purchase == null)
            {
                return ResponseDto<PurchaseDto>.CreateErrorResponse("Purchase not found.", ApiErrorCode.NotFound);
            }
            return ResponseDto<PurchaseDto>.CreateSuccessResponse(_mapper.Map<PurchaseDto>(purchase));
        }
    }
}

