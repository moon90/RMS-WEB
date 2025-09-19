using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.Promotions;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        [Authorize(Policy = "PROMOTION_VIEW")]
        public async Task<IActionResult> GetAllPromotions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, [FromQuery] bool? status = null)
        {
            try
            {
                var result = await _promotionService.GetAllPromotionsAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status);
                var response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Promotions retrieved successfully",
                    Code = "200",
                    Data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving promotions.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "PROMOTION_VIEW")]
        public async Task<IActionResult> GetPromotionById(int id)
        {
            try
            {
                var result = await _promotionService.GetPromotionByIdAsync(id);
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
                    Message = "An error occurred while retrieving the promotion.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("by-code/{couponCode}")]
        [Authorize(Policy = "PROMOTION_VIEW")]
        public async Task<IActionResult> GetPromotionByCouponCode(string couponCode)
        {
            try
            {
                var result = await _promotionService.GetPromotionByCouponCodeAsync(couponCode);
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
                    Message = "An error occurred while retrieving the promotion by code.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Policy = "PROMOTION_CREATE")]
        public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionDto promotionDto)
        {
            try
            {
                var result = await _promotionService.CreatePromotionAsync(promotionDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return CreatedAtAction(nameof(GetPromotionById), new { id = result.Data.PromotionID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the promotion.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "PROMOTION_UPDATE")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] UpdatePromotionDto promotionDto)
        {
            try
            {
                if (id != promotionDto.PromotionID)
                {
                    return BadRequest("Promotion ID mismatch.");
                }
                var result = await _promotionService.UpdatePromotionAsync(id, promotionDto);
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
                    Message = "An error occurred while updating the promotion.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "PROMOTION_DELETE")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            try
            {
                var result = await _promotionService.DeletePromotionAsync(id);
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
                    Message = "An error occurred while deleting the promotion.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "PROMOTION_TOGGLE_STATUS")]
        public async Task<IActionResult> UpdatePromotionStatus(int id, [FromBody] PromotionStatusUpdateDto dto)
        {
            try
            {
                var result = await _promotionService.UpdateStatusAsync(id, dto.Status);
                if (!result.IsSuccess)
                {
                    if (result.Code == "404") return NotFound(result);
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the promotion status.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}