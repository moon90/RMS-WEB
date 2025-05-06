using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos.PermissionDTOs.InputDTOs;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
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
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        // GET: api/permission/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Permission ID must be greater than 0.");

            try
            {
                var permission = await _permissionService.GetPermissionByIdAsync(id);
                return Ok(permission);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/permission
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PermissionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _permissionService.CreatePermissionAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // PUT: api/permission/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PermissionUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Mismatch between route ID and payload ID.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _permissionService.UpdatePermissionAsync(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/permission/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Permission ID must be greater than 0.");

            try
            {
                await _permissionService.DeletePermissionAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
