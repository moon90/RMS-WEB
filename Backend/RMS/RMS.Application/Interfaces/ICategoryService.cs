using RMS.Application.DTOs.CategoryDTOs.InputDTOs;
using RMS.Application.DTOs.CategoryDTOs.OutputDTOs;
using RMS.Domain.Dtos;
using RMS.Domain.Models.BaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<ResponseDto<CategoryDto>> GetByIdAsync(int id);
        Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
        Task<PagedResult<CategoryDto>> GetAllCategoriesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<CategoryDto>> CreateAsync(CategoryCreateDto createDto);
        Task<ResponseDto<CategoryDto>> UpdateAsync(CategoryUpdateDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
    }
}