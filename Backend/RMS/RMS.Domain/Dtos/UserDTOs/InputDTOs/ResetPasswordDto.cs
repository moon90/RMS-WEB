using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Dtos.UserDTOs.InputDTOs
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string ResetToken { get; set; } // optional: can be GUID or any secret
        public string NewPassword { get; set; }
    }
}
