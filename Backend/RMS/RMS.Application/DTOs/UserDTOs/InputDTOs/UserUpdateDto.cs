namespace RMS.Application.DTOs.UserDTOs.InputDTOs
{
    public class UserUpdateDto
    {
        public int UserID { get; set; } // Matches User.Id
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool Status { get; set; }
        

        public string? ProfilePictureUrl { get; set; } // Update profile image
    }
}
