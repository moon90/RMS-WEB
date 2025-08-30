using RMS.Application.DTOs.MenuDTOs.OutputDTOs;

namespace RMS.Application.DTOs.UserDTOs.OutputDTOs
{
    public class UserMenuPermissionDto : MenuDto
    {
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}