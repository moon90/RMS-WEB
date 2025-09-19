namespace RMS.Application.DTOs.StaffDTOs.InputDTOs
{
    public class UpdateStaffDto
    {
        public int StaffID { get; set; }

        public string? StaffName { get; set; }

        public string? StaffPhone { get; set; }

        public string? StaffRole { get; set; }

        public bool Status { get; set; }
    }
}
