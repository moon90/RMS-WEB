
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Core.Enum;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Implementations
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PurchaseService> _logger;

        public PurchaseService(
            IPurchaseRepository purchaseRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<PurchaseService> logger)
        {
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<List<PurchaseDto>>> GetAllPurchasesAsync()
        {
            var purchases = await _purchaseRepository.GetQueryable()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();

            return ResponseDto<List<PurchaseDto>>.CreateSuccessResponse(_mapper.Map<List<PurchaseDto>>(purchases));
        }

        public async Task<ResponseDto<PurchaseDto>> CreatePurchaseAsync(CreatePurchaseDto purchaseDto)
        {
            var purchase = _mapper.Map<Purchase>(purchaseDto);
            purchase.PurchaseDate = DateTime.UtcNow;
            purchase.CreatedDate = DateTime.UtcNow;
            purchase.CreatedBy = "system";

            var newPurchase = await _purchaseRepository.AddAsync(purchase);
            await _unitOfWork.SaveChangesAsync();

            return ResponseDto<PurchaseDto>.CreateSuccessResponse(_mapper.Map<PurchaseDto>(newPurchase), "Purchase recorded successfully.", "201");
        }

        public async Task<ResponseDto<PurchaseDto>> GetPurchaseByIdAsync(int id)
        {
            var purchase = await _purchaseRepository.GetQueryable()
                .Include(p => p.PurchaseDetails)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.PurchaseID == id);

            if (purchase == null)
            {
                return ResponseDto<PurchaseDto>.CreateErrorResponse("Purchase not found.", ApiErrorCode.NotFound);
            }
            return ResponseDto<PurchaseDto>.CreateSuccessResponse(_mapper.Map<PurchaseDto>(purchase));
        }

        public async Task<ResponseDto<string>> CheckAndGenerateAutoPOsAsync()
        {
            try
            {
                var ingredients = await _unitOfWork.GetRepository<Ingredient>().GetQueryable()
                    .Where(i => i.QuantityAvailable <= i.ReorderLevel && i.Status && !i.IsDeleted)
                    .ToListAsync();

                if (!ingredients.Any()) return ResponseDto<string>.CreateSuccessResponse("Stock levels optimal.");

                var groupedBySupplier = ingredients.GroupBy(i => i.SupplierID);

                foreach (var group in groupedBySupplier)
                {
                    if (group.Key == null) continue;

                    var existingDraft = await _purchaseRepository.GetQueryable()
                        .Include(p => p.PurchaseDetails)
                        .FirstOrDefaultAsync(p => p.SupplierID == group.Key && p.PurchaseStatus == PurchaseStatus.Draft && p.CreatedDate.Date == DateTime.UtcNow.Date);

                    if (existingDraft != null)
                    {
                        foreach (var ing in group)
                        {
                            if (existingDraft.PurchaseDetails.Any(pd => pd.IngredientID == ing.IngredientID)) continue;

                            existingDraft.PurchaseDetails.Add(new PurchaseDetail
                            {
                                IngredientID = ing.IngredientID,
                                Quantity = (int)ing.ReorderQuantity,
                                UnitPrice = ing.CostPrice,
                                TotalAmount = ing.ReorderQuantity * ing.CostPrice,
                                CreatedBy = "AI_COPILOT",
                                CreatedDate = DateTime.UtcNow
                            });
                        }
                        await _purchaseRepository.UpdateAsync(existingDraft);
                    }
                    else
                    {
                        var newPurchase = new Purchase
                        {
                            SupplierID = group.Key.Value,
                            PurchaseDate = DateTime.UtcNow,
                            PurchaseStatus = PurchaseStatus.Draft,
                            PaymentMethod = "Credit",
                            CategoryId = 1,
                            Remarks = "Auto-generated by AI Co-Pilot due to low stock.",
                            CreatedBy = "AI_COPILOT",
                            CreatedDate = DateTime.UtcNow,
                            PurchaseDetails = group.Select(ing => new PurchaseDetail
                            {
                                IngredientID = ing.IngredientID,
                                Quantity = (int)ing.ReorderQuantity,
                                UnitPrice = ing.CostPrice,
                                TotalAmount = ing.ReorderQuantity * ing.CostPrice,
                                CreatedBy = "AI_COPILOT",
                                CreatedDate = DateTime.UtcNow
                            }).ToList()
                        };
                        newPurchase.TotalAmount = newPurchase.PurchaseDetails.Sum(pd => pd.TotalAmount);
                        await _purchaseRepository.AddAsync(newPurchase);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                return ResponseDto<string>.CreateSuccessResponse("Auto-POs synchronized.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Auto-PO engine failed.");
                return ResponseDto<string>.CreateErrorResponse("Internal Engine Error");
            }
        }
    }
}
