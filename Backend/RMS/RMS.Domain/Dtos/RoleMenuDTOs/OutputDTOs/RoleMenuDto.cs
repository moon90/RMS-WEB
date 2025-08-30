using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs.RoleMenuDTOs.OutputDTOs
{
    public class RoleMenuDto
    {
        public int RoleMenuID { get; set; }
        public int RoleID { get; set; }
        public int MenuID { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public string? menuName { get; set; }
        public int DisplayOrder { get; set; } // Added
    }
}
