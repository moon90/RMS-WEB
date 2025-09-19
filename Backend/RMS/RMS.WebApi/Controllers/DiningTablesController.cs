using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs;
using RMS.Application.DTOs.DiningTables;
using RMS.Application.Interfaces;
using RMS.Domain.Models.BaseModels; // For Result<T>
using RMS.Domain.Queries; // For PagedQuery
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DiningTablesController : ControllerBase
    {
        private readonly IDiningTableService _diningTableService;

        public DiningTablesController(IDiningTableService diningTableService)
        {
            _diningTableService = diningTableService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "DINING_TABLE_VIEW")]
        public async Task<IActionResult> GetDiningTableById(int id)
        {
            try
            {
                var result = await _diningTableService.GetDiningTableByIdAsync(id);
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
                    Message = "An error occurred while retrieving the dining table.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet]
        [Authorize(Policy = "DINING_TABLE_VIEW")]
        public async Task<IActionResult> GetAllDiningTables([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null)
        {
            try
            {
                var result = await _diningTableService.GetAllDiningTablesAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection);
                var response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Dining Tables retrieved successfully",
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
                    Message = "An error occurred while retrieving dining tables.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Policy = "DINING_TABLE_CREATE")]
        public async Task<IActionResult> CreateDiningTable([FromBody] CreateDiningTableDto diningTableDto)
        {
            try
            {
                var result = await _diningTableService.CreateDiningTableAsync(diningTableDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return CreatedAtAction(nameof(GetDiningTableById), new { id = result.Data.TableID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the dining table.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "DINING_TABLE_UPDATE")]
        public async Task<IActionResult> UpdateDiningTable(int id, [FromBody] UpdateDiningTableDto diningTableDto)
        {
            try
            {
                if (id != diningTableDto.TableID)
                {
                    return BadRequest("Dining Table ID mismatch.");
                }
                var result = await _diningTableService.UpdateDiningTableAsync(id, diningTableDto);
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
                    Message = "An error occurred while updating the dining table.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("status")]
        [Authorize(Policy = "DINING_TABLE_UPDATE")]
        public async Task<IActionResult> UpdateDiningTableStatus([FromBody] UpdateDiningTableStatusDto diningTableStatusDto)
        {
            try
            {
                var result = await _diningTableService.UpdateDiningTableStatusAsync(diningTableStatusDto.TableID, diningTableStatusDto.Status);
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
                    Message = "An error occurred while updating the dining table status.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DINING_TABLE_DELETE")]
        public async Task<IActionResult> DeleteDiningTable(int id)
        {
            try
            {
                var result = await _diningTableService.DeleteDiningTableAsync(id);
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
                    Message = "An error occurred while deleting the dining table.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}