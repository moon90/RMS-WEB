using AutoMapper;
using RMS.Application.DTOs.Promotions;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Infrastructure.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RMS.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using RMS.Application.DTOs;
using RMS.Core.Enum;

namespace RMS.Application.Implementations
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromotionService(IPromotionRepository promotionRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _promotionRepository = promotionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto<PagedResult<PromotionDto>>> GetAllPromotionsAsync(int pageNumber, int pageSize, string searchQuery, string sortColumn, string sortDirection, bool? status)
        {
            var query = _promotionRepository.GetQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.CouponCode.Contains(searchQuery) || p.Description.Contains(searchQuery));
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.IsActive == status.Value);
            }

            query = query.ApplySort(sortColumn, sortDirection);

            var pagedPromotions = await query.ToPagedList(pageNumber, pageSize);
            var promotionDtos = _mapper.Map<List<PromotionDto>>(pagedPromotions.Items);

            return ResponseDto<PagedResult<PromotionDto>>.CreateSuccessResponse(new PagedResult<PromotionDto>
            {
                Items = promotionDtos,
                TotalRecords = pagedPromotions.TotalRecords,
                PageNumber = pagedPromotions.PageNumber,
                PageSize = pagedPromotions.PageSize
            });
        }

        public async Task<ResponseDto<PromotionDto>> GetPromotionByIdAsync(int id)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null)
            {
                return ResponseDto<PromotionDto>.CreateErrorResponse("Promotion not found.", ApiErrorCode.NotFound);
            }
            return ResponseDto<PromotionDto>.CreateSuccessResponse(_mapper.Map<PromotionDto>(promotion));
        }

        public async Task<ResponseDto<PromotionDto>> GetPromotionByCouponCodeAsync(string couponCode)
        {
            var promotion = await _promotionRepository.GetPromotionByCouponCodeAsync(couponCode);
            if (promotion == null)
            {
                return ResponseDto<PromotionDto>.CreateErrorResponse("Promotion not found.", ApiErrorCode.NotFound);
            }
            return ResponseDto<PromotionDto>.CreateSuccessResponse(_mapper.Map<PromotionDto>(promotion));
        }

        public async Task<ResponseDto<PromotionDto>> CreatePromotionAsync(CreatePromotionDto createPromotionDto)
        {
            var promotion = _mapper.Map<Promotion>(createPromotionDto);
            await _promotionRepository.AddAsync(promotion);
            await _unitOfWork.CommitTransactionAsync();
            return ResponseDto<PromotionDto>.CreateSuccessResponse(_mapper.Map<PromotionDto>(promotion));
        }

        public async Task<ResponseDto<PromotionDto>> UpdatePromotionAsync(int id, UpdatePromotionDto updatePromotionDto)
        {
            var existingPromotion = await _promotionRepository.GetByIdAsync(id);
            if (existingPromotion == null)
            {
                return ResponseDto<PromotionDto>.CreateErrorResponse("Promotion not found.", ApiErrorCode.NotFound);
            }

            _mapper.Map(updatePromotionDto, existingPromotion);
            await _promotionRepository.UpdateAsync(existingPromotion);
            await _unitOfWork.CommitTransactionAsync();
            return ResponseDto<PromotionDto>.CreateSuccessResponse(_mapper.Map<PromotionDto>(existingPromotion));
        }

        public async Task<ResponseDto<bool>> DeletePromotionAsync(int id)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null)
            {
                return ResponseDto<bool>.CreateErrorResponse("Promotion not found.", ApiErrorCode.NotFound);
            }

            await _promotionRepository.DeleteAsync(promotion);
            await _unitOfWork.CommitTransactionAsync();
            return ResponseDto<bool>.CreateSuccessResponse(true);
        }

        public async Task<ResponseDto<bool>> UpdateStatusAsync(int id, bool status)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null)
            {
                return ResponseDto<bool>.CreateErrorResponse("Promotion not found.", ApiErrorCode.NotFound);
            }

            promotion.IsActive = status;

            await _promotionRepository.UpdateAsync(promotion);
            await _unitOfWork.CommitTransactionAsync();

            return ResponseDto<bool>.CreateSuccessResponse(true);
        }
    }
}