using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using System.Threading.Tasks;
using RMS.Application.DTOs.Orders;

namespace RMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KitchenController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public KitchenController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders/{statuses?}")]
        public async Task<IActionResult> GetKitchenOrders(string? statuses = "Pending,Preparing")
        {
            var allOrders = new List<OrderDto>();
            var statusList = statuses?.Split(',').ToList() ?? new List<string> { "Pending", "Preparing" };

            foreach (var status in statusList)
            {
                var response = await _orderService.GetAllOrdersAsync(1, 100, null, null, null, status.Trim());
                if (response.IsSuccess && response.Data != null && response.Data.Items != null)
                {
                    allOrders.AddRange(response.Data.Items);
                }
                else if (!response.IsSuccess)
                {
                    // Log or handle individual status fetch errors if necessary
                    // For now, we'll just continue to the next status
                }
            }

            if (!allOrders.Any() && statusList.Any())
            {
                return NotFound($"No orders found for statuses: {statuses}");
            }

            return Ok(new { Items = allOrders, TotalCount = allOrders.Count });
        }

        [HttpPost("orders/{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto statusDto)
        {
            var response = await _orderService.UpdateOrderStatusAsync(id, statusDto.OrderStatus); // Assuming this method exists
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
    }
}
