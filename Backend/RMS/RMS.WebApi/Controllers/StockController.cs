using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.StockTransactionDTOs.InputDTOs;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockTransactionService _stockTransactionService;

        public StockController(IStockTransactionService stockTransactionService)
        {
            _stockTransactionService = stockTransactionService;
        }

        [HttpPost("receive")]
        [Authorize(Policy = "STOCK_RECEIVE")]
        public async Task<IActionResult> ReceiveStock([FromBody] ReceiveStockDto receiveStockDto)
        {
            try
            {
                var createDto = new CreateStockTransactionDto
                {
                    ProductID = receiveStockDto.ProductID,
                    Quantity = receiveStockDto.Quantity,
                    SupplierID = receiveStockDto.SupplierID,
                    Remarks = receiveStockDto.Remarks,
                    TransactionType = "IN",
                    TransactionDate = DateTime.UtcNow
                };

                var result = await _stockTransactionService.ProcessStockTransactionAsync(createDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Stock received successfully.",
                    Code = "200"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while receiving stock.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("adjust")]
        [Authorize(Policy = "STOCK_ADJUST")]
        public async Task<IActionResult> AdjustStock([FromBody] AdjustStockDto adjustStockDto)
        {
            try
            {
                var createDto = new CreateStockTransactionDto
                {
                    ProductID = adjustStockDto.ProductID,
                    Quantity = Math.Abs(adjustStockDto.QuantityChange),
                    Remarks = adjustStockDto.Remarks,
                    TransactionType = adjustStockDto.QuantityChange > 0 ? "IN" : "OUT",
                    TransactionDate = DateTime.UtcNow
                };

                var result = await _stockTransactionService.ProcessStockTransactionAsync(createDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Stock adjusted successfully.",
                    Code = "200"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adjusting stock.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}