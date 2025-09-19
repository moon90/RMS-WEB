
using RMS.Application.DTOs.ManufacturerDTOs.InputDTOs;
using RMS.Application.DTOs.ManufacturerDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;

namespace RMS.Application.Interfaces
{
    public interface IManufacturerService
    {
        Task<ResponseDto<ManufacturerDto>> GetByIdAsync(int id);
        Task<PagedResult<ManufacturerDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<ManufacturerDto>> CreateAsync(CreateManufacturerDto createDto);
        Task<ResponseDto<ManufacturerDto>> UpdateAsync(UpdateManufacturerDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
    }
}
