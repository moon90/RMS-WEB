using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs.RoleDTOs.OutputDTOs
{
    public class RoleDto
    {
        public int RoleID { get; set; }
        public required string RoleName { get; set; }
        public required string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
