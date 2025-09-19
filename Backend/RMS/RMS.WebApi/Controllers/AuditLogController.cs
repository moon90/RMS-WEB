using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs;
using RMS.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Authorization;
using RMS.Application.Interfaces;

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
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Audit logs retrieved successfully",
                    Code = "200",
                    Data = result
                });
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
                // This method will now use the paginated GetAllAuditLogsAsync internally if needed, or be refactored.
                // For now, I'll leave it as is, but it's a potential area for improvement to use the new paginated method.
                var logs = await _auditLogService.GetAllAuditLogsAsync(1, 1000, entityType, "PerformedAt", "desc"); // Example: fetch all for entity type
                var filteredLogs = logs.Items.Where(l => l.EntityType == entityType).ToList();

                if (!filteredLogs.Any())
                {
                    return NotFound(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = $"No audit logs found for entity type {entityType}.",
                        Code = "404"
                    });
                }
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = $"Audit logs for entity type {entityType} retrieved successfully",
                    Code = "200",
                    Data = filteredLogs
                });
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
                // This method will now use the paginated GetAllAuditLogsAsync internally if needed, or be refactored.
                // For now, I'll leave it as is, but it's a potential area for improvement to use the new paginated method.
                var logs = await _auditLogService.GetAllAuditLogsAsync(1, 1000, performedBy, "PerformedAt", "desc"); // Example: fetch all for user
                var filteredLogs = logs.Items.Where(l => l.PerformedBy == performedBy).ToList();

                if (!filteredLogs.Any())
                {
                    return NotFound(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        Message = $"No audit logs found for user {performedBy}.",
                        Code = "404"
                    });
                }
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = $"Audit logs for user {performedBy} retrieved successfully",
                    Code = "200",
                    Data = filteredLogs
                });
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