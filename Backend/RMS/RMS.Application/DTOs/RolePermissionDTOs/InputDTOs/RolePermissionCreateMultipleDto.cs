using System.Collections.Generic;
using RMS.Application.Interfaces;

namespace RMS.Application.DTOs.RolePermissionDTOs.InputDTOs
{
    public class RolePermissionCreateMultipleDto
    {
        public int RoleID { get; set; }
        public List<int> PermissionIDs { get; set; }
        public int? SortingOrder { get; set; }
    }
}
