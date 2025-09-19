using RMS.Application.DTOs.InventoryDTOs.InputDTOs;
using RMS.Application.DTOs.InventoryDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<ResponseDto<InventoryDto>> GetByIdAsync(int id);
        Task<PagedResult<InventoryDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<InventoryDto>> CreateAsync(CreateInventoryDto createDto);
        Task<ResponseDto<InventoryDto>> UpdateAsync(UpdateInventoryDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
        Task<PagedResult<InventoryDto>> GetLowStockProductsAsync(int pageNumber, int pageSize);
    }
}
