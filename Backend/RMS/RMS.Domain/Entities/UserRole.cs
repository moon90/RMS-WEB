using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Entities
{
    public class UserRole
    {
        public int Id { get; set; }

        public int UserID { get; set; }
        public User? User { get; set; }

        public int RoleID { get; set; }
        public Role? Role { get; set; }

        public DateTime AssignedAt { get; set; }
        public string AssignedBy { get; set; }
    }
}
