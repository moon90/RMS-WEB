using RMS.Application.DTOs.StaffDTOs.InputDTOs;
using RMS.Application.DTOs.StaffDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IStaffService
    {
        Task<ResponseDto<StaffDto>> GetByIdAsync(int id);
        Task<PagedResult<StaffDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<StaffDto>> CreateAsync(CreateStaffDto createDto);
        Task<ResponseDto<StaffDto>> UpdateAsync(UpdateStaffDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
    }
}
