using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IAuditLogService
    {
        Task LogAsync(string action, string entityType, string entityId, string performedBy, string details = null);
    }
}
