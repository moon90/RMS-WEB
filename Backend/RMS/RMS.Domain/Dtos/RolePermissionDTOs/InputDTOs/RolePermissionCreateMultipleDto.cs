using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs.RolePermissionDTOs.InputDTOs
{
    public class RolePermissionCreateMultipleDto
    {
        public int RoleID { get; set; }
        public List<int> PermissionIDs { get; set; }
        public int? SortingOrder { get; set; }
    }
}
