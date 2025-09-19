using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using RMS.Application.DTOs.PermissionDTOs.InputDTOs;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        // GET: api/permission
        [HttpGet]
        [Authorize(Policy = "PERMISSION_VIEW")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, [FromQuery] bool? status = null)
        {
            try
            {
                var result = await _permissionService.GetAllPermissionsAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection, status);

                var response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Permissions retrieved successfully",
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
                    Message = "An error occurred while retrieving permissions.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // GET: api/permission/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "PERMISSION_VIEW")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Permission ID must be greater than 0.",
                    Code = "400"
                });

            try
            {
                var response = await _permissionService.GetPermissionByIdAsync(id);
                if (!response.IsSuccess)
                    return NotFound(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred.",
                    Code = "500",
                    Details = ex.Message
                });
            }
        }

        // POST: api/permission
        [HttpPost]
        [Authorize(Policy = "PERMISSION_CREATE")]
        public async Task<IActionResult> Create([FromBody] PermissionCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Validation failed.",
                        Code = "400",
                        Details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                var response = await _permissionService.CreatePermissionAsync(dto);
                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }
                return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the permission.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // PUT: api/permission/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "PERMISSION_UPDATE")]
        public async Task<IActionResult> Update(int id, [FromBody] PermissionUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Mismatch between route ID and payload ID.",
                        Code = "400"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = "Validation failed.",
                        Code = "400",
                        Details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                var response = await _permissionService.UpdatePermissionAsync(dto);
                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the permission.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // DELETE: api/permission/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "PERMISSION_DELETE")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Permission ID must be greater than 0.",
                    Code = "400"
                });
            }

            try
            {
                var response = await _permissionService.DeletePermissionAsync(id);
                if (!response.IsSuccess)
                    return NotFound(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the permission.",
                    Code = "500",
                    Details = ex.Message
                });
            }
        }
    }

}
