namespace RMS.Application.DTOs.UserRoleDTOs.OutputDTOs
{
    public class UserRoleDto
    {
        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime AssignedAt { get; set; }
        public string? AssignedBy { get; set; }
    }
}
