using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.Dtos.PermissionDTOs.InputDTOs;

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
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _permissionService.GetAllPermissionsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving permissions.",
                    Code = "500",
                    Details = ex.Message
                });
            }
        }

        // GET: api/permission/{id}
        [HttpGet("{id}")]
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
        public async Task<IActionResult> Create([FromBody] PermissionCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Validation failed.",
                    Code = "400",
                    Details = ModelState
                });
            }

            try
            {
                var response = await _permissionService.CreatePermissionAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = response.Data }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the permission.",
                    Code = "500",
                    Details = ex.Message
                });
            }
        }

        // PUT: api/permission/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PermissionUpdateDto dto)
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
                    Details = ModelState
                });
            }

            try
            {
                var response = await _permissionService.UpdatePermissionAsync(dto);
                if (!response.IsSuccess)
                    return NotFound(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the permission.",
                    Code = "500",
                    Details = ex.Message
                });
            }
        }

        // DELETE: api/permission/{id}
        [HttpDelete("{id}")]
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
