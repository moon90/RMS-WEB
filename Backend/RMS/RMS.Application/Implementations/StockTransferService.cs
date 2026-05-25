using RMS.Infrastructure.IRepositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs;
using RMS.Domain.Interfaces;
using RMS.Core.Enum;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMS.Application.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Extensions;

namespace RMS.Application.Implementations
{
    public class StockTransferService : IStockTransferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;
        private readonly ILogger<StockTransferService> _logger;

        public StockTransferService(
            IUnitOfWork unitOfWork,
            ITenantService tenantService,
            IMapper mapper,
            ILogger<StockTransferService> logger)
        {
            _unitOfWork = unitOfWork;
            _tenantService = tenantService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDto<PagedResult<StockTransferDto>>> GetAllTransfersAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection)
        {
            var query = _unitOfWork.GetRepository<StockTransfer>().GetQueryable()
                .Include(t => t.FromBranch)
                .Include(t => t.ToBranch)
                .Include(t => t.Details)
                    .ThenInclude(d => d.Ingredient)
                .AsQueryable();

            // Custom multi-tenant logic: Show if user is From or To branch
            if (_tenantService.BranchID.HasValue)
            {
                var bid = _tenantService.BranchID.Value;
                query = query.Where(t => t.FromBranchID == bid || t.ToBranchID == bid);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(t => t.TransferNumber.Contains(searchQuery) || 
                                         t.FromBranch.BranchName.Contains(searchQuery) || 
                                         t.ToBranch.BranchName.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.ApplySort(sortColumn, sortDirection ?? "asc");
            }
            else
            {
                query = query.OrderByDescending(t => t.TransferDate);
            }

            var pagedResult = await query.ToPagedList(pageNumber, pageSize);
            var dtos = _mapper.Map<List<StockTransferDto>>(pagedResult.Items);

            var result = new PagedResult<StockTransferDto>(dtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
            return ResponseDto<PagedResult<StockTransferDto>>.CreateSuccessResponse(result);
        }

        public async Task<ResponseDto<StockTransferDto>> GetTransferByIdAsync(int id)
        {
            var transfer = await _unitOfWork.GetRepository<StockTransfer>().GetQueryable()
                .Include(t => t.FromBranch)
                .Include(t => t.ToBranch)
                .Include(t => t.Details)
                    .ThenInclude(d => d.Ingredient)
                .FirstOrDefaultAsync(t => t.StockTransferID == id);

            if (transfer == null) return ResponseDto<StockTransferDto>.CreateErrorResponse("Transfer not found.");
            return ResponseDto<StockTransferDto>.CreateSuccessResponse(_mapper.Map<StockTransferDto>(transfer));
        }

        public async Task<ResponseDto<StockTransferDto>> CreateTransferAsync(CreateStockTransferDto transferDto)
        {
            if (!_tenantService.BranchID.HasValue) 
                return ResponseDto<StockTransferDto>.CreateErrorResponse("Unauthorized: Branch context missing.");

            int fromBranchId = _tenantService.BranchID.Value;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var transfer = new StockTransfer
                {
                    FromBranchID = fromBranchId,
                    ToBranchID = transferDto.ToBranchID,
                    TransferDate = DateTime.UtcNow,
                    Status = TransferStatus.Shipped, // Direct to shipped for simplicity in this version
                    Remarks = transferDto.Remarks,
                    TransferNumber = $"TRF-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}",
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow
                };

                foreach (var detailDto in transferDto.Details)
                {
                    var sourceIngredient = await _unitOfWork.GetRepository<Ingredient>().GetByIdAsync(detailDto.IngredientID);
                    if (sourceIngredient == null || sourceIngredient.BranchID != fromBranchId)
                        throw new Exception($"Ingredient ID {detailDto.IngredientID} not found in source branch.");

                    if (sourceIngredient.QuantityAvailable < detailDto.Quantity)
                        throw new Exception($"Insufficient stock for '{sourceIngredient.Name}'. Available: {sourceIngredient.QuantityAvailable}");

                    // 1. Deduct from Source
                    sourceIngredient.QuantityAvailable -= detailDto.Quantity;
                    await _unitOfWork.GetRepository<Ingredient>().UpdateAsync(sourceIngredient);

                    // 2. Add Detail
                    transfer.Details.Add(new StockTransferDetail
                    {
                        IngredientID = sourceIngredient.IngredientID,
                        Quantity = detailDto.Quantity,
                        UnitID = sourceIngredient.UnitID,
                        CreatedBy = "system",
                        CreatedDate = DateTime.UtcNow
                    });
                }

                var newTransfer = await _unitOfWork.GetRepository<StockTransfer>().AddAsync(transfer);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ResponseDto<StockTransferDto>.CreateSuccessResponse(_mapper.Map<StockTransferDto>(newTransfer), "Stock transfer initiated and shipped.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Stock transfer failed.");
                return ResponseDto<StockTransferDto>.CreateErrorResponse(ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> UpdateTransferStatusAsync(int id, string status)
        {
            var transfer = await _unitOfWork.GetRepository<StockTransfer>().GetQueryable()
                .Include(t => t.Details)
                    .ThenInclude(d => d.Ingredient)
                .FirstOrDefaultAsync(t => t.StockTransferID == id);

            if (transfer == null) return ResponseDto<bool>.CreateErrorResponse("Transfer not found.");

            // Security: Only target branch can mark as Received
            if (status == "Received" && _tenantService.BranchID != transfer.ToBranchID)
                return ResponseDto<bool>.CreateErrorResponse("Only the destination branch can acknowledge receipt.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (status == "Received" && transfer.Status != TransferStatus.Received)
                {
                    foreach (var detail in transfer.Details)
                    {
                        // Match ingredient in target branch by name
                        var targetIngredient = await _unitOfWork.GetRepository<Ingredient>().GetQueryable()
                            .FirstOrDefaultAsync(i => i.BranchID == transfer.ToBranchID && i.Name == detail.Ingredient.Name);

                        if (targetIngredient == null)
                        {
                            // Auto-create ingredient in target branch if it doesn't exist
                            targetIngredient = new Ingredient
                            {
                                Name = detail.Ingredient.Name,
                                BranchID = transfer.ToBranchID,
                                QuantityAvailable = 0,
                                UnitID = detail.UnitID,
                                CostPrice = detail.Ingredient.CostPrice,
                                ReorderLevel = detail.Ingredient.ReorderLevel,
                                ReorderQuantity = detail.Ingredient.ReorderQuantity,
                                CreatedBy = "SYSTEM_TRANSFER"
                            };
                            await _unitOfWork.GetRepository<Ingredient>().AddAsync(targetIngredient);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        targetIngredient.QuantityAvailable += detail.Quantity;
                        await _unitOfWork.GetRepository<Ingredient>().UpdateAsync(targetIngredient);
                    }
                    transfer.Status = TransferStatus.Received;
                }
                else if (status == "Cancelled" && transfer.Status == TransferStatus.Shipped)
                {
                    // Return stock to source
                    foreach (var detail in transfer.Details)
                    {
                        var sourceIngredient = await _unitOfWork.GetRepository<Ingredient>().GetByIdAsync(detail.IngredientID);
                        sourceIngredient.QuantityAvailable += detail.Quantity;
                        await _unitOfWork.GetRepository<Ingredient>().UpdateAsync(sourceIngredient);
                    }
                    transfer.Status = TransferStatus.Cancelled;
                }

                await _unitOfWork.GetRepository<StockTransfer>().UpdateAsync(transfer);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ResponseDto<bool>.CreateSuccessResponse(true, $"Transfer status updated to {status}.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ResponseDto<bool>.CreateErrorResponse(ex.Message);
            }
        }
    }
}
