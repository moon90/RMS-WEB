namespace RMS.Application.DTOs.RoleDTOs.InputDTOs
{
    public class RoleUpdateDto
    {
        public int RoleID { get; set; }
        public required string RoleName { get; set; }
        public required string Description { get; set; }
        public bool Status { get; set; }
    }
}
