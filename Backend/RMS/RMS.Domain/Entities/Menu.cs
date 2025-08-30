using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Entities
{
    public class Menu : BaseEntity
    {
        public int Id { get; set; }
        public required string MenuName { get; set; }
        public int? ParentID { get; set; } // Self-referencing for hierarchical menus
        public required string MenuPath { get; set; }
        public required string MenuIcon { get; set; }
        public required string ControllerName { get; set; }
        public required string ActionName { get; set; }
        public required string ModuleName { get; set; }
        public int DisplayOrder { get; set; }

        // Navigation properties
        public Menu? ParentMenu { get; set; }
        public ICollection<Menu> ChildMenus { get; set; } = new List<Menu>();
        public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
    }
}
