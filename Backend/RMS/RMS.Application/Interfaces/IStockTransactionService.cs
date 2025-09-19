using RMS.Application.DTOs.StockTransactionDTOs.InputDTOs;
using RMS.Application.DTOs.StockTransactionDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IStockTransactionService
    {
        Task<ResponseDto<StockTransactionDto>> GetByIdAsync(int id);
        Task<PagedResult<StockTransactionDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<StockTransactionDto>> CreateAsync(CreateStockTransactionDto createDto);
        Task<ResponseDto<StockTransactionDto>> UpdateAsync(UpdateStockTransactionDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
        Task<ResponseDto<StockTransactionDto>> ProcessStockTransactionAsync(CreateStockTransactionDto createDto);
    }
}
