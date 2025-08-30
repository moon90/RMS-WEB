
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.ProductDTOs.InputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // Added
using RMS.WebApi.Services; // Added
using System; // Added for Exception

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IImageService _imageService; // Added

        public ProductsController(IProductService productService, IImageService imageService) // Modified
        {
            _productService = productService;
            _imageService = imageService; // Added
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "PRODUCT_VIEW")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _productService.GetByIdAsync(id);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the product.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpGet]
        [Authorize(Policy = "PRODUCT_VIEW")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, [FromQuery] bool? status = null)
        {
            try
            {
                var result = await _productService.GetAllAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status);

                var response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Products retrieved successfully",
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
                    Message = "An error occurred while retrieving products.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Policy = "PRODUCT_CREATE")]
        public async Task<IActionResult> Create([FromForm] CreateProductDto createDto, IFormFile? productImageFile, IFormFile? thumbnailImageFile)
        {
            try
            {
                if (productImageFile != null)
                {
                    createDto.ImageUrl = await _imageService.SaveImageAsync(productImageFile, "products");
                }
                if (thumbnailImageFile != null)
                {
                    createDto.ThumbnailUrl = await _imageService.SaveImageAsync(thumbnailImageFile, "products");
                }

                var result = await _productService.CreateAsync(createDto);
                if (!result.IsSuccess)
                {
                    // If creation fails, delete uploaded images
                    if (!string.IsNullOrEmpty(createDto.ImageUrl)) _imageService.DeleteImage(createDto.ImageUrl);
                    if (!string.IsNullOrEmpty(createDto.ThumbnailUrl)) _imageService.DeleteImage(createDto.ThumbnailUrl);
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
            }
            catch (ArgumentException ex) // Catch specific validation exception
            {
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = ex.Message, // Use the message from the ArgumentException
                    Code = "IMAGE_VALIDATION_ERROR",
                    Details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the product.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "PRODUCT_UPDATE")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDto updateDto, IFormFile? productImageFile, IFormFile? thumbnailImageFile)
        {
            try
            {
                if (id != updateDto.Id)
                {
                    return BadRequest(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Product ID mismatch.",
                        Code = "ID_MISMATCH"
                    });
                }

                // Get existing product to check old image URLs
                var existingProductResult = await _productService.GetByIdAsync(id);
                if (!existingProductResult.IsSuccess || existingProductResult.Data == null)
                {
                    return NotFound(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        Message = "Product not found.",
                        Code = "PRODUCT_NOT_FOUND"
                    });
                }
                var existingProduct = existingProductResult.Data;

                // Handle Product Image
                if (productImageFile != null)
                {
                    // Save new image
                    var newImageUrl = await _imageService.SaveImageAsync(productImageFile, "products");
                    if (!string.IsNullOrEmpty(newImageUrl))
                    {
                        // Delete old image if it exists
                        if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                        {
                            _imageService.DeleteImage(existingProduct.ImageUrl);
                        }
                        updateDto.ImageUrl = newImageUrl;
                    }
                }
                else if (string.IsNullOrEmpty(updateDto.ImageUrl) && !string.IsNullOrEmpty(existingProduct.ImageUrl))
                {
                    // If no new file is provided and ImageUrl is explicitly cleared in DTO, delete old image
                    _imageService.DeleteImage(existingProduct.ImageUrl);
                }

                // Handle Thumbnail Image
                if (thumbnailImageFile != null)
                {
                    // Save new thumbnail
                    var newThumbnailUrl = await _imageService.SaveImageAsync(thumbnailImageFile, "products");
                    if (!string.IsNullOrEmpty(newThumbnailUrl))
                    {
                        // Delete old thumbnail if it exists
                        if (!string.IsNullOrEmpty(existingProduct.ThumbnailUrl))
                        {
                            _imageService.DeleteImage(existingProduct.ThumbnailUrl);
                        }
                        updateDto.ThumbnailUrl = newThumbnailUrl;
                    }
                }
                else if (string.IsNullOrEmpty(updateDto.ThumbnailUrl) && !string.IsNullOrEmpty(existingProduct.ThumbnailUrl))
                {
                    // If no new file is provided and ThumbnailUrl is explicitly cleared in DTO, delete old thumbnail
                    _imageService.DeleteImage(existingProduct.ThumbnailUrl);
                }

                var result = await _productService.UpdateAsync(updateDto);
                if (!result.IsSuccess)
                {
                    // If update fails, and new images were uploaded, delete them to prevent orphaned files
                    if (productImageFile != null && !string.IsNullOrEmpty(updateDto.ImageUrl))
                    {
                        _imageService.DeleteImage(updateDto.ImageUrl);
                    }
                    if (thumbnailImageFile != null && !string.IsNullOrEmpty(updateDto.ThumbnailUrl))
                    {
                        _imageService.DeleteImage(updateDto.ThumbnailUrl);
                    }
                    return BadRequest(result);
                }

                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (ArgumentException ex) // Catch specific validation exception
            {
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = ex.Message, // Use the message from the ArgumentException
                    Code = "IMAGE_VALIDATION_ERROR",
                    Details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the product.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "PRODUCT_DELETE")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _productService.DeleteAsync(id);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the product.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "PRODUCT_UPDATE")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ProductStatusUpdateDto dto)
        {
            try
            {
                var result = await _productService.UpdateStatusAsync(id, dto.Status);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the product status.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}
