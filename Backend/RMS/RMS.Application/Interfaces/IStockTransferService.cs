using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IStockTransferService
    {
        Task<ResponseDto<PagedResult<StockTransferDto>>> GetAllTransfersAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection);
        Task<ResponseDto<StockTransferDto>> GetTransferByIdAsync(int id);
        Task<ResponseDto<StockTransferDto>> CreateTransferAsync(CreateStockTransferDto transferDto);
        Task<ResponseDto<bool>> UpdateTransferStatusAsync(int id, string status);
    }
}
