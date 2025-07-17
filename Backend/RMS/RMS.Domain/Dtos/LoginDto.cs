using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
