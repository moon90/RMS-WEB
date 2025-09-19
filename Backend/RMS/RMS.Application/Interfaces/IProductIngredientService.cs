using RMS.Application.DTOs.ProductIngredientDTOs.InputDTOs;
using RMS.Application.DTOs.ProductIngredientDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IProductIngredientService
    {
        Task<ResponseDto<ProductIngredientDto>> GetByIdAsync(int id);
        Task<PagedResult<ProductIngredientDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<ProductIngredientDto>> CreateAsync(CreateProductIngredientDto createDto);
        Task<ResponseDto<ProductIngredientDto>> UpdateAsync(UpdateProductIngredientDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
    }
}
