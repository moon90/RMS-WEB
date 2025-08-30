using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public List<string>? RolePermissions { get; set; }
        public List<MenuPermissionDto>? MenuPermissions { get; set; }
    }
}
