using RMS.Application.DTOs.UnitDTOs.InputDTOs;
using RMS.Application.DTOs.UnitDTOs.OutputDTOs;
using RMS.Domain.Dtos;
using RMS.Domain.Models.BaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IUnitService
    {
        Task<ResponseDto<UnitDto>> GetByIdAsync(int id);
        Task<PagedResult<UnitDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<UnitDto>> CreateAsync(CreateUnitDto createDto);
        Task<ResponseDto<UnitDto>> UpdateAsync(UpdateUnitDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
    }
}
