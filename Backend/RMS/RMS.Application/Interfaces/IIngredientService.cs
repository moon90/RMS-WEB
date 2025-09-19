using RMS.Application.DTOs.IngredientDTOs.InputDTOs;
using RMS.Application.DTOs.IngredientDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IIngredientService
    {
        Task<ResponseDto<IngredientDto>> GetByIdAsync(int id);
        Task<PagedResult<IngredientDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<IngredientDto>> CreateAsync(CreateIngredientDto createDto);
        Task<ResponseDto<IngredientDto>> UpdateAsync(UpdateIngredientDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
    }
}
