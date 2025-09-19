using System.Collections.Generic;

namespace RMS.Application.DTOs
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public List<string>? RolePermissions { get; set; }
        public List<MenuPermissionDto>? MenuPermissions { get; set; }
    }
}