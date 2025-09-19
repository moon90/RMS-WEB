using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.IngredientDTOs.InputDTOs;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "INGREDIENT_VIEW")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _ingredientService.GetByIdAsync(id);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the ingredient.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet]
        [Authorize(Policy = "INGREDIENT_VIEW")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, [FromQuery] bool? status = null)
        {
            try
            {
                var result = await _ingredientService.GetAllAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status);

                var response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Ingredients retrieved successfully",
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
                    Message = "An error occurred while retrieving ingredients.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Policy = "INGREDIENT_CREATE")]
        public async Task<IActionResult> Create([FromBody] CreateIngredientDto createDto)
        {
            try
            {
                var result = await _ingredientService.CreateAsync(createDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(GetById), new { id = result.Data.IngredientID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the ingredient.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "INGREDIENT_UPDATE")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateIngredientDto updateDto)
        {
            try
            {
                if (id != updateDto.IngredientID)
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Ingredient ID mismatch.",
                        Code = "ID_MISMATCH"
                    });
                }

                var result = await _ingredientService.UpdateAsync(updateDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the ingredient.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "INGREDIENT_DELETE")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _ingredientService.DeleteAsync(id);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the ingredient.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "INGREDIENT_UPDATE")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateIngredientDto dto)
        {
            try
            {
                var result = await _ingredientService.UpdateStatusAsync(id, dto.Status);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the ingredient status.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}
