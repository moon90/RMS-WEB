namespace RMS.Application.DTOs.RolePermissionDTOs.InputDTOs
{
    public class RolePermissionCreateDto
    {
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public int? SortingOrder { get; set; }
    }
}
