using RMS.Infrastructure.IRepositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs;
using RMS.Domain.Interfaces;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMS.Application.Interfaces;

namespace RMS.Application.Implementations
{
    public class InventoryAuditService : IInventoryAuditService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryAuditService> _logger;

        public InventoryAuditService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<InventoryAuditService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDto<List<InventoryAuditDto>>> GetAllAuditsAsync()
        {
            var audits = await _unitOfWork.GetRepository<InventoryAudit>().GetQueryable()
                .Include(a => a.Details)
                .OrderByDescending(a => a.AuditDate)
                .ToListAsync();

            return ResponseDto<List<InventoryAuditDto>>.CreateSuccessResponse(_mapper.Map<List<InventoryAuditDto>>(audits));
        }

        public async Task<ResponseDto<InventoryAuditDto>> GetAuditByIdAsync(int id)
        {
            var audit = await _unitOfWork.GetRepository<InventoryAudit>().GetQueryable()
                .Include(a => a.Details)
                    .ThenInclude(d => d.Ingredient)
                .FirstOrDefaultAsync(a => a.InventoryAuditID == id);

            if (audit == null) return ResponseDto<InventoryAuditDto>.CreateErrorResponse("Audit not found.");
            return ResponseDto<InventoryAuditDto>.CreateSuccessResponse(_mapper.Map<InventoryAuditDto>(audit));
        }

        public async Task<ResponseDto<InventoryAuditDto>> CreateAuditAsync(CreateInventoryAuditDto auditDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var audit = new InventoryAudit
                {
                    AuditDate = DateTime.UtcNow,
                    AuditorName = auditDto.AuditorName,
                    Remarks = auditDto.Remarks,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow
                };

                foreach (var detailDto in auditDto.Details)
                {
                    var ingredient = await _unitOfWork.GetRepository<Ingredient>().GetByIdAsync(detailDto.IngredientID);
                    if (ingredient == null) continue;

                    var detail = new InventoryAuditDetail
                    {
                        IngredientID = ingredient.IngredientID,
                        TheoreticalStock = ingredient.QuantityAvailable,
                        PhysicalStock = detailDto.PhysicalStock,
                        VarianceValue = (detailDto.PhysicalStock - ingredient.QuantityAvailable) * ingredient.CostPrice,
                        CreatedBy = "system",
                        CreatedDate = DateTime.UtcNow
                    };

                    audit.Details.Add(detail);

                    // AI Action: Adjust actual stock to match physical count
                    ingredient.QuantityAvailable = detailDto.PhysicalStock;
                    await _unitOfWork.GetRepository<Ingredient>().UpdateAsync(ingredient);
                }

                var newAudit = await _unitOfWork.GetRepository<InventoryAudit>().AddAsync(audit);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ResponseDto<InventoryAuditDto>.CreateSuccessResponse(_mapper.Map<InventoryAuditDto>(newAudit), "Inventory Audit completed. Variance AI updated.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Inventory audit failed.");
                return ResponseDto<InventoryAuditDto>.CreateErrorResponse(ex.Message);
            }
        }
    }
}
