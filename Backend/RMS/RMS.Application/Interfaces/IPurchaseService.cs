
using RMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IPurchaseService
    {
        Task<ResponseDto<PurchaseDto>> GetPurchaseByIdAsync(int id);
        Task<ResponseDto<List<PurchaseDto>>> GetAllPurchasesAsync();
        Task<ResponseDto<PurchaseDto>> CreatePurchaseAsync(CreatePurchaseDto purchaseDto);
    }
}
