using System.Collections.Generic;

namespace RMS.Application.DTOs.RoleMenuDTOs.InputDTOs
{
    public class RoleMenuBulkUpdateDto
    {
        public int RoleId { get; set; }
        public List<int> MenuIds { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}