using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using RMS.Application.DTOs;
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
        [Authorize(Policy = "KITCHEN_VIEW")]
        public async Task<IActionResult> GetKitchenOrders(string? statuses = "Pending,Preparing")
        {
            try
            {
                var response = await _orderService.GetAllOrdersAsync(1, 1000, null, "OrderTime", "asc", statuses);
                
                if (response.IsSuccess && response.Data != null)
                {
                    return Ok(new { Items = response.Data.Items, TotalCount = response.Data.TotalRecords });
                }

                return Ok(new { Items = new List<OrderDto>(), TotalCount = 0 });
            }
            catch (Exception ex)
            {
                // Fallback or log error
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpPost("orders/{id}/status")]
        [Authorize(Policy = "KITCHEN_UPDATE")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto statusDto)
        {
            var response = await _orderService.UpdateOrderStatusAsync(id, statusDto.OrderStatus, statusDto.ChefID);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
