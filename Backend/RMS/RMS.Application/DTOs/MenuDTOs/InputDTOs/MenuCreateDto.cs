namespace RMS.Application.DTOs.MenuDTOs.InputDTOs
{
    public class MenuCreateDto
    {
        public required string MenuName { get; set; }
        public int? ParentID { get; set; }
        public required string MenuPath { get; set; }
        public required string MenuIcon { get; set; }
        public required string ControllerName { get; set; }
        public required string ActionName { get; set; }
        public required string ModuleName { get; set; }
        public int DisplayOrder { get; set; }
    }
}