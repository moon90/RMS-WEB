using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs.MenuDTOs.InputDTOs
{
    public class MenuUpdateDto
    {
        public int MenuID { get; set; }
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
