using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs.UserDTOs.InputDTOs
{
    public class UserCreateDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; } // Plain password
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool Status { get; set; } = true; // Default to active

        public string? ProfilePictureUrl { get; set; } // Optional profile picture
        //public int RoleID { get; set; }
    }
}
