using RMS.Domain.Interfaces;

namespace RMS.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        public int? BranchID { get; set; }
    }
}
