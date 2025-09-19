namespace RMS.Application.DTOs
{
    public class AuthResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public object? User { get; set; }
        public object? RolePermissions { get; set; }
        public object? MenuPermissions { get; set; }
    }
}
