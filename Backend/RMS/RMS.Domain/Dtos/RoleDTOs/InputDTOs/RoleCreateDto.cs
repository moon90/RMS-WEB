using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs.RoleDTOs.InputDTOs
{
    public class RoleCreateDto
    {
        public required string RoleName { get; set; }
        public required string Description { get; set; }
    }
}
