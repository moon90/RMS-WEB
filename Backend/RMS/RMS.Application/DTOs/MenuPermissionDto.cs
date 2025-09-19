namespace RMS.Application.DTOs
{
    public class MenuPermissionDto
    {
        public int MenuID { get; set; }
        public string? MenuName { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}