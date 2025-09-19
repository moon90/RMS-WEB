namespace RMS.Application.DTOs.UserDTOs.InputDTOs
{
    public class ResetPasswordDto
    {
        public required string Email { get; set; }
        public required string ResetToken { get; set; } // optional: can be GUID or any secret
        public required string NewPassword { get; set; }
    }
}
