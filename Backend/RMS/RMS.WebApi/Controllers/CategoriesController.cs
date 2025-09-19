using RMS.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Application.DTOs.CategoryDTOs.InputDTOs;
using Microsoft.AspNetCore.Authorization;

namespace RMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Policy = "CATEGORY_VIEW")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, [FromQuery] bool? status = null)
        {
            try
            {
                var result = await _categoryService.GetAllCategoriesAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status);
                var response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Categories retrieved successfully",
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
                    Message = "An error occurred while retrieving categories.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "CATEGORY_VIEW")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _categoryService.GetByIdAsync(id);
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
                    Message = "An error occurred while retrieving the category.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Policy = "CATEGORY_CREATE")]
        public async Task<IActionResult> Create(CategoryCreateDto createDto)
        {
            try
            {
                var result = await _categoryService.CreateAsync(createDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return CreatedAtAction(nameof(GetById), new { id = result.Data.CategoryID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the category.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "CATEGORY_UPDATE")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto updateDto)
        {
            try
            {
                if (id != updateDto.CategoryID)
                {
                    return BadRequest("Category ID mismatch.");
                }
                var result = await _categoryService.UpdateAsync(updateDto);
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
                    Message = "An error occurred while updating the category.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "CATEGORY_DELETE")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _categoryService.DeleteAsync(id);
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
                    Message = "An error occurred while deleting the category.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "CATEGORY_TOGGLE_STATUS")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] CategoryStatusUpdateDto dto)
        {
            try
            {
                var result = await _categoryService.UpdateStatusAsync(id, dto.Status);
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
                    Message = "An error occurred while updating the category status.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}