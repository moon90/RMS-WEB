using RMS.Application.DTOs.Promotions;
using RMS.Domain.Models.BaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using RMS.Application.DTOs;

namespace RMS.Application.Interfaces
{
    public interface IPromotionService
    {
        Task<ResponseDto<PagedResult<PromotionDto>>> GetAllPromotionsAsync(int pageNumber, int pageSize, string searchQuery, string sortColumn, string sortDirection, bool? status);
        Task<ResponseDto<PromotionDto>> GetPromotionByIdAsync(int id);
        Task<ResponseDto<PromotionDto>> GetPromotionByCouponCodeAsync(string couponCode);
        Task<ResponseDto<PromotionDto>> CreatePromotionAsync(CreatePromotionDto createPromotionDto);
        Task<ResponseDto<PromotionDto>> UpdatePromotionAsync(int id, UpdatePromotionDto updatePromotionDto);
        Task<ResponseDto<bool>> DeletePromotionAsync(int id);
        Task<ResponseDto<bool>> UpdateStatusAsync(int id, bool status);
    }
}
