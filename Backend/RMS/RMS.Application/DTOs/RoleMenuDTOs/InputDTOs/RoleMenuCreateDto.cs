namespace RMS.Application.DTOs.RoleMenuDTOs.InputDTOs
{
    public class RoleMenuCreateDto
    {
        public int RoleID { get; set; }
        public int MenuID { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
