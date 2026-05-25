using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class Staff : BaseEntity, IMultiTenant
    {
        public int StaffID { get; set; }
        public string? StaffName { get; set; }
        public string? StaffPhone { get; set; }
        public string? StaffRole { get; set; }
        public decimal HourlyRate { get; set; } = 0;
        public decimal CommissionPercentage { get; set; } = 0; // % bonus on total revenue generated
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}
