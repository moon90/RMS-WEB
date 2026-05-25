using RMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IInventoryAuditService
    {
        Task<ResponseDto<List<InventoryAuditDto>>> GetAllAuditsAsync();
        Task<ResponseDto<InventoryAuditDto>> GetAuditByIdAsync(int id);
        Task<ResponseDto<InventoryAuditDto>> CreateAuditAsync(CreateInventoryAuditDto auditDto);
    }
}
