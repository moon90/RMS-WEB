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
    [Route("api/auditlogs")]
    [Route("api/audit-logs")]
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
        public async Task<IActionResult> GetAll([FromQuery] int? lastSeenId = null, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _auditLogService.GetAllAuditLogsAsync(lastSeenId, pageSize, searchQuery, sortColumn, sortDirection, cancellationToken);
                return Ok(new ResponseDto<RMS.Domain.Models.BaseModels.KeysetPagedResult<RMS.Application.DTOs.AuditLogs.AuditLogDto>>
                {
                    IsSuccess = true,
                    Message = "Audit logs retrieved successfully.",
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
                var logs = await _auditLogService.GetAllAuditLogsAsync(null, 1000, entityType, "PerformedAt", "desc");
                return Ok(new ResponseDto<RMS.Domain.Models.BaseModels.KeysetPagedResult<RMS.Application.DTOs.AuditLogs.AuditLogDto>>
                {
                    IsSuccess = true,
                    Message = "Audit logs retrieved successfully.",
                    Data = logs
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
                var logs = await _auditLogService.GetAllAuditLogsAsync(null, 1000, performedBy, "PerformedAt", "desc");
                return Ok(new ResponseDto<RMS.Domain.Models.BaseModels.KeysetPagedResult<RMS.Application.DTOs.AuditLogs.AuditLogDto>>
                {
                    IsSuccess = true,
                    Message = "Audit logs retrieved successfully.",
                    Data = logs
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