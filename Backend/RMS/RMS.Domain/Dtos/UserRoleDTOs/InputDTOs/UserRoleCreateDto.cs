using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs.UserRoleDTOs.InputDTOs
{
    public class UserRoleCreateDto
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}
