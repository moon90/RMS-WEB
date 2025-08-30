using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Dtos.UserDTOs.InputDTOs
{
    public class ForgetPasswordDto
    {
        public required string Email { get; set; }
    }
}
