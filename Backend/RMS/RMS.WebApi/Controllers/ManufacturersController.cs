
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.ManufacturerDTOs.InputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturersController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "MANUFACTURER_VIEW")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _manufacturerService.GetByIdAsync(id);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the manufacturer.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet]
        [Authorize(Policy = "MANUFACTURER_VIEW")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, [FromQuery] bool? status = null)
        {
            try
            {
                var result = await _manufacturerService.GetAllAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status);

                var response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Manufacturers retrieved successfully",
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
                    Message = "An error occurred while retrieving manufacturers.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Policy = "MANUFACTURER_CREATE")]
        public async Task<IActionResult> Create([FromBody] CreateManufacturerDto createDto)
        {
            try
            {
                var result = await _manufacturerService.CreateAsync(createDto);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the manufacturer.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "MANUFACTURER_UPDATE")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateManufacturerDto updateDto)
        {
            try
            {
                if (id != updateDto.Id)
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Manufacturer ID mismatch.",
                        Code = "ID_MISMATCH"
                    });
                }

                var result = await _manufacturerService.UpdateAsync(updateDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the manufacturer.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "MANUFACTURER_DELETE")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _manufacturerService.DeleteAsync(id);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the manufacturer.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "MANUFACTURER_UPDATE")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ManufacturerStatusUpdateDto dto)
        {
            try
            {
                var result = await _manufacturerService.UpdateStatusAsync(id, dto.Status);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the manufacturer status.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}
