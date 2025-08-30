using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Models.Request
{
    public class AssignUserRoleRequest
    {
        public required string UserId { get; set; }
        public required string RoleId { get; set; }
    }
}
