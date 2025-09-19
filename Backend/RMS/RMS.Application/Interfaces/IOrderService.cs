using RMS.Application.DTOs.Orders;
using RMS.Application.DTOs;
using RMS.Domain.Models.BaseModels;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseDto<OrderDto>> GetOrderByIdAsync(int id);
        Task<ResponseDto<PagedResult<OrderDto>>> GetAllOrdersAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, string? status);
        Task<ResponseDto<OrderDto>> CreateOrderAsync(CreateOrderDto orderDto);
        Task<ResponseDto<OrderDto>> UpdateOrderAsync(int id, UpdateOrderDto orderDto);
        Task<ResponseDto<bool>> DeleteOrderAsync(int id);
        Task<ResponseDto<OrderDto>> ProcessPaymentForOrderAsync(ProcessPaymentForOrderDto paymentDto);
        Task<ResponseDto<OrderDto>> UpdateOrderStatusAsync(int id, string orderStatus);
    }
}