namespace RMS.Application.DTOs.UserRoleDTOs.InputDTOs
{
    public class UserRoleCreateDto
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public DateTime AssignedAt { get; set; }
        public required string AssignedBy { get; set; }
    }
}
