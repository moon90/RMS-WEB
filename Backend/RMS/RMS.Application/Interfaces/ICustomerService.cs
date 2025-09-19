using RMS.Application.DTOs.CustomerDTOs.InputDTOs;
using RMS.Application.DTOs.CustomerDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<ResponseDto<CustomerDto>> GetByIdAsync(int id);
        Task<PagedResult<CustomerDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status);
        Task<ResponseDto<CustomerDto>> CreateAsync(CreateCustomerDto createDto);
        Task<ResponseDto<CustomerDto>> UpdateAsync(UpdateCustomerDto updateDto);
        Task<ResponseDto<string>> DeleteAsync(int id);
        Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status);
    }
}
