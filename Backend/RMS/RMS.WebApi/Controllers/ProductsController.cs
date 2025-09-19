
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RMS.Application.DTOs.ProductDTOs.InputDTOs;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using RMS.WebApi.Configurations;
using RMS.Application.Services.Processing; // Added for Exception

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly ImageSettings _imageSettings;

        public ProductsController(IProductService productService, IImageProcessingService imageProcessingService, IOptions<ImageSettings> imageSettings) // Modified
        {
            _productService = productService;
            _imageProcessingService = imageProcessingService;
            _imageSettings = imageSettings.Value;
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
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, [FromQuery] bool? status = null, [FromQuery] int? categoryId = null)
        {
            try
            {
                var result = await _productService.GetAllAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status, categoryId);

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
                    var imageBytes = await _imageProcessingService.ProcessImage(productImageFile, _imageSettings.ProductImageWidth, _imageSettings.ProductImageHeight);
                    createDto.ProductImage = ConvertBytesToBase64(imageBytes);
                }
                if (thumbnailImageFile != null)
                {
                    var thumbnailBytes = await _imageProcessingService.ProcessImage(thumbnailImageFile, _imageSettings.ThumbnailImageWidth, _imageSettings.ThumbnailImageHeight);
                    createDto.ThumbnailImage = ConvertBytesToBase64(thumbnailBytes);
                }

                var result = await _productService.CreateAsync(createDto);
                if (!result.IsSuccess)
                {
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

                // Fetch the existing product to preserve image data if no new file is uploaded
                var existingProductResult = await _productService.GetByIdAsync(id);
                if (!existingProductResult.IsSuccess || existingProductResult.Data == null)
                {
                    return NotFound(new ResponseDto<string> { IsSuccess = false, Message = "Product not found.", Code = "404" });
                }
                var existingProduct = existingProductResult.Data;

                // Handle ProductImage
                if (productImageFile != null)
                {
                    var imageBytes = await _imageProcessingService.ProcessImage(productImageFile, _imageSettings.ProductImageWidth, _imageSettings.ProductImageHeight);
                    updateDto.ProductImage = ConvertBytesToBase64(imageBytes);
                }
                else if (updateDto.ProductImage == "") // If client sends empty string, clear the image
                {
                    updateDto.ProductImage = null;
                }
                else if (updateDto.ProductImage == null) // If no new file, and DTO has no Base64 string, retain the existing one from DB
                {
                    updateDto.ProductImage = existingProduct.ProductImage;
                }
                // else: updateDto.ProductImage already contains a Base64 string (either existing or new from client) - do nothing

                // Handle ThumbnailImage
                if (thumbnailImageFile != null)
                {
                    var thumbnailBytes = await _imageProcessingService.ProcessImage(thumbnailImageFile, _imageSettings.ThumbnailImageWidth, _imageSettings.ThumbnailImageHeight);
                    updateDto.ThumbnailImage = ConvertBytesToBase64(thumbnailBytes);
                }
                else if (updateDto.ThumbnailImage == "") // If client sends empty string, clear the image
                {
                    updateDto.ThumbnailImage = null;
                }
                else if (updateDto.ThumbnailImage == null) // If no new file, and DTO has no Base64 string, retain the existing one from DB
                {
                    updateDto.ThumbnailImage = existingProduct.ThumbnailImage;
                }
                // else: updateDto.ThumbnailImage already contains a Base64 string (either existing or new from client) - do nothing

                var result = await _productService.UpdateAsync(updateDto);
                if (!result.IsSuccess)
                {
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
    private string? ConvertBytesToBase64(byte[]? bytes)
        {
            return bytes != null ? $"data:image/png;base64,{Convert.ToBase64String(bytes)}" : null;
        }
    }
}
