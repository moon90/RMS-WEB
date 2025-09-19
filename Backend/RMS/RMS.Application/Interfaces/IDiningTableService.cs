using RMS.Application.DTOs.DiningTables;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IDiningTableService
    {
        Task<ResponseDto<DiningTableDto>> GetDiningTableByIdAsync(int id);
        Task<ResponseDto<PagedResult<DiningTableDto>>> GetAllDiningTablesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection);
        Task<ResponseDto<DiningTableDto>> CreateDiningTableAsync(CreateDiningTableDto diningTableDto);
        Task<ResponseDto<DiningTableDto>> UpdateDiningTableAsync(int id, UpdateDiningTableDto diningTableDto);
        Task<ResponseDto<bool>> UpdateDiningTableStatusAsync(int id, bool status);
        Task<ResponseDto<bool>> DeleteDiningTableAsync(int id);
    }
}