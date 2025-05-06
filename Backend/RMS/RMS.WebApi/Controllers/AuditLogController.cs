using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Infrastructure.IRepositories;

namespace RMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogController(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        // Get all logs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _auditLogRepository.GetAllAsync();
            return Ok(logs);
        }

        // Filter by entity type
        [HttpGet("entity/{entityType}")]
        public async Task<IActionResult> GetByEntityType(string entityType)
        {
            var logs = await _auditLogRepository.GetByEntityTypeAsync(entityType);
            return Ok(logs);
        }

        // Filter by user
        [HttpGet("user/{performedBy}")]
        public async Task<IActionResult> GetByUser(string performedBy)
        {
            var logs = await _auditLogRepository.GetByUserAsync(performedBy);
            return Ok(logs);
        }
    }
}
