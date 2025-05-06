using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs.MenuDTOs.InputDTOs
{
    public class MenuCreateDto
    {
        public string MenuName { get; set; }
        public int? ParentID { get; set; }
        public string MenuPath { get; set; }
        public string MenuIcon { get; set; }
        public int DisplayOrder { get; set; }
    }
}
