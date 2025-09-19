namespace RMS.Domain.Entities
{
    public class Staff : BaseEntity
    {
        public int StaffID { get; set; }
        public string? StaffName { get; set; }
        public string? StaffPhone { get; set; }
        public string? StaffRole { get; set; }
    }
}
