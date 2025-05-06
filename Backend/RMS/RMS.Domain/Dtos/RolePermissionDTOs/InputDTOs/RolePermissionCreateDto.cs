using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs.RolePermissionDTOs.InputDTOs
{
    public class RolePermissionCreateDto
    {
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public int? SortingOrder { get; set; }
    }
}
