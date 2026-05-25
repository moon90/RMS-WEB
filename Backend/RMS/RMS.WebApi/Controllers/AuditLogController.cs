using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using RMS.Domain.Interfaces;

namespace RMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply Authorize to the whole controller
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        // Get all logs with pagination
        [HttpGet]
        [Authorize(Policy = "AUDIT_LOG_VIEW")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null)
        {
            try
            {
                var result = await _auditLogService.GetAllAuditLogsAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving audit logs.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Filter by entity type
        [HttpGet("entity/{entityType}")]
        [Authorize(Policy = "AUDIT_LOG_VIEW")]
        public async Task<IActionResult> GetByEntityType(string entityType)
        {
            try
            {
                var logs = await _auditLogService.GetAllAuditLogsAsync(1, 1000, entityType, "PerformedAt", "desc");
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving audit logs by entity type.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }

        // Filter by user
        [HttpGet("user/{performedBy}")]
        [Authorize(Policy = "AUDIT_LOG_VIEW")]
        public async Task<IActionResult> GetByUser(string performedBy)
        {
            try
            {
                var logs = await _auditLogService.GetAllAuditLogsAsync(1, 1000, performedBy, "PerformedAt", "desc");
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving audit logs by user.",
                    Code = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message
                });
            }
        }
    }
}