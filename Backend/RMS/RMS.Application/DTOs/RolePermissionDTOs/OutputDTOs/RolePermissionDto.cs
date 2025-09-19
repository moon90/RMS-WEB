namespace RMS.Application.DTOs.RolePermissionDTOs.OutputDTOs
{
    public class RolePermissionDto
    {
        public int RolePermissionID { get; set; }
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public int? SortingOrder { get; set; }
        public DateTime AssignedAt { get; set; }
        public string? AssignedBy { get; set; }
    }
}
