
using RMS.Application.DTOs.ProductDTOs.InputDTOs;
using RMS.Application.DTOs.ProductDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;

namespace RMS.Application.Interfaces
{
    public interface IProductService
    {
        Task<ResponseDto<ProductDto>> GetByIdAsync(int id);
        Task<PagedResult<ProductDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status, int? categoryId);
        Task<ResponseDto<ProductDto>> CreateAsync(CreateProductDto createDto);
        Task<ResponseDto<ProductDto>> UpdateAsync(UpdateProductDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
        Task<ResponseDto<string>> ConsumeIngredientsForProductAsync(int productId, int quantitySold);
    }
}
