
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
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateSaleDto> _createSaleValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<SaleService> _logger;
        private readonly IProductService _productService;

        public SaleService(
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateSaleDto> createSaleValidator,
            IAuditLogService auditLogService,
            ILogger<SaleService> logger,
            IProductService productService)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createSaleValidator = createSaleValidator;
            _auditLogService = auditLogService;
            _logger = logger;
            _productService = productService;
        }

        public async Task<ResponseDto<SaleDto>> CreateSaleAsync(CreateSaleDto saleDto)
        {
            var validationResult = await _createSaleValidator.ValidateAsync(saleDto);
            if (!validationResult.IsValid)
            {
                return ResponseDto<SaleDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var sale = _mapper.Map<Sale>(saleDto);
                var newSale = await _saleRepository.AddAsync(sale);

                foreach (var detail in saleDto.SaleDetails)
                {
                    var inventory = await _inventoryRepository.GetByProductIdAsync(detail.ProductId);

                    if (inventory != null)
                    {
                        if (inventory.CurrentStock < detail.Quantity)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ResponseDto<SaleDto>.CreateErrorResponse($"Not enough stock for Product ID: {detail.ProductId}", ApiErrorCode.BadRequest);
                        }

                        inventory.CurrentStock -= detail.Quantity;
                        await _inventoryRepository.UpdateAsync(inventory);

                        // Consume ingredients for the sold product
                        var consumeResult = await _productService.ConsumeIngredientsForProductAsync(detail.ProductId, detail.Quantity);
                        if (!consumeResult.IsSuccess)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ResponseDto<SaleDto>.CreateErrorResponse(consumeResult.Message, ApiErrorCode.BadRequest);
                        }
                    }
                    else
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ResponseDto<SaleDto>.CreateErrorResponse($"Inventory not found for Product ID: {detail.ProductId}", ApiErrorCode.NotFound);
                    }
                }

                await _unitOfWork.CommitTransactionAsync();

                await _auditLogService.LogAsync("CreateSale", "Sale", $"SaleId:{newSale.SaleID}", "System", $"Sale created for customer '{newSale.CustomerID}'.");

                return ResponseDto<SaleDto>.CreateSuccessResponse(_mapper.Map<SaleDto>(newSale), "Sale created successfully.", "201");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "An error occurred while creating the sale.");
                return ResponseDto<SaleDto>.CreateErrorResponse("An error occurred while creating the sale.", ApiErrorCode.ServerError);
            }
        }

        public async Task<List<SaleDto>> GetSalesAsync()
        {
            var spec = new SaleWithCategorySpecification();
            var sales = await _saleRepository.GetBySpecAsync(spec);
            return _mapper.Map<List<SaleDto>>(sales);
        }

        public async Task<SaleDto> GetSaleAsync(int id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            return _mapper.Map<SaleDto>(sale);
        }
    }
}
