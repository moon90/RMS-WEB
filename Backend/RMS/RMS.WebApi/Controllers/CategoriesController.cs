using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.CategoryDTOs.InputDTOs;
using RMS.Application.DTOs.CategoryDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Policy = "CATEGORY_VIEW")]
        public async Task<IActionResult> GetAllCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, [FromQuery] bool? status = null)
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
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<CategoryDto>
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
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            try
            {
                var result = await _categoryService.CreateAsync(dto);
                if (!result.IsSuccess)
                    return BadRequest(result);

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
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {
            try
            {
                if (id != dto.CategoryID)
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Category ID mismatch.",
                        Code = "ID_MISMATCH"
                    });
                }

                var result = await _categoryService.UpdateAsync(dto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
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
                return result.IsSuccess ? Ok(result) : NotFound(result);
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
        [Authorize(Policy = "CATEGORY_UPDATE")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] CategoryStatusUpdateDto dto)
        {
            try
            {
                var result = await _categoryService.UpdateStatusAsync(id, dto.Status);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
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