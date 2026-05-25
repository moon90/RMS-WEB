using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface ISaleService
    {
        Task<ResponseDto<PagedResult<SaleDto>>> GetAllSalesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection);
        Task<ResponseDto<SaleDto>> GetSaleAsync(int id);
        Task<ResponseDto<SaleDto>> CreateSaleAsync(CreateSaleDto saleDto);
        Task<ResponseDto<SaleDto>> UpdateSaleAsync(int id, CreateSaleDto saleDto);
        Task<ResponseDto<bool>> DeleteSaleAsync(int id);
    }
}
