using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RMS.Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [JsonPropertyName("userName")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [JsonPropertyName("password")]
        public required string Password { get; set; }
    }
}
