using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs.UserDTOs.InputDTOs
{
    public class UserUpdateDto
    {
        public int UserID { get; set; } // Matches `User.Id`
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool Status { get; set; }
        public int RoleID { get; set; }
    }
}
