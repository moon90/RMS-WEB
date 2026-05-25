using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.Orders;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
using System.Threading.Tasks;
using RMS.Domain.Queries;
using RMS.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ORDER_VIEW")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var result = await _orderService.GetOrderByIdAsync(id);
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the order.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet]
        [Authorize(Policy = "ORDER_VIEW")]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, [FromQuery] string? status = null)
        {
            try
            {
                var result = await _orderService.GetAllOrdersAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var detailedError = ex.Message;
                if (ex.InnerException != null) detailedError += " | Inner: " + ex.InnerException.Message;

                return StatusCode(500, new 
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving orders.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = detailedError,
                    StackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost]
        [Authorize(Policy = "ORDER_CREATE")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            try
            {
                var result = await _orderService.CreateOrderAsync(orderDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return CreatedAtAction(nameof(GetOrderById), new { id = result.Data.OrderID }, result);
            }
            catch (Exception ex)
            {
                var detailedError = ex.Message;
                if (ex.InnerException != null) detailedError += " | Inner: " + ex.InnerException.Message;

                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = $"Backend Error: {detailedError}",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.StackTrace
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ORDER_UPDATE")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto orderDto)
        {
            try
            {
                if (id != orderDto.OrderID)
                {
                    return BadRequest("Order ID mismatch.");
                }
                var result = await _orderService.UpdateOrderAsync(id, orderDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the order.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ORDER_DELETE")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrderAsync(id);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the order.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("process-payment")]
        [Authorize(Policy = "ORDER_UPDATE")]
        public async Task<IActionResult> ProcessPaymentForOrder([FromBody] ProcessPaymentForOrderDto paymentDto)
        {
            try
            {
                var result = await _orderService.ProcessPaymentForOrderAsync(paymentDto);
                if (!result.IsSuccess)
                {
                    // Log the failure details for easier debugging
                    Console.WriteLine($"Payment processing failed for Order {paymentDto.OrderID}: {result.Message}");
                    if (result.Details != null)
                    {
                        Console.WriteLine($"Details: {System.Text.Json.JsonSerializer.Serialize(result.Details)}");
                    }
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error in ProcessPaymentForOrder: {ex.Message}");
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing the payment.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}