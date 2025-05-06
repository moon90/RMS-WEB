using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Entities
{
    public class RolePermission
    {
        public int Id { get; set; }

        public int RoleID { get; set; }
        public Role? Role { get; set; }

        public int PermissionID { get; set; }
        public Permission? Permission { get; set; }

        public DateTime AssignedAt { get; set; }
        public string AssignedBy { get; set; }
    }
}
