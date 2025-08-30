using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Domain.Dtos;
using RMS.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Authorization;

namespace RMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply Authorize to the whole controller
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogController(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        // Get all logs
        [HttpGet]
        [Authorize(Policy = "AUDIT_LOG_VIEW")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var logs = await _auditLogRepository.GetAllAsync();
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Audit logs retrieved successfully",
                    Code = "200",
                    Data = logs
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
                var logs = await _auditLogRepository.GetByEntityTypeAsync(entityType);
                if (logs == null || !logs.Any())
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
                var logs = await _auditLogRepository.GetByUserAsync(performedBy);
                if (logs == null || !logs.Any())
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