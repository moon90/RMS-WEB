
using RMS.Application.DTOs.SupplierDTOs.InputDTOs;
using RMS.Application.DTOs.SupplierDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;

namespace RMS.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<ResponseDto<SupplierDto>> GetByIdAsync(int id);
        Task<PagedResult<SupplierDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<SupplierDto>> CreateAsync(CreateSupplierDto createDto);
        Task<ResponseDto<SupplierDto>> UpdateAsync(UpdateSupplierDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
    }
}
