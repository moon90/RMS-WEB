using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Entities
{
    public class RoleMenu
    {
        public int Id { get; set; }
        public int RoleID { get; set; }
        public int MenuID { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        // Navigation properties
        public Role? Role { get; set; }
        public Menu? Menu { get; set; }

        public DateTime AssignedAt { get; set; }
        public string AssignedBy { get; set; }
    }
}
