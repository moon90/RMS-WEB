using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs
{
    public class ErrorResponseDto
    {
        public required string Message { get; set; } // Main error message
        public required string ErrorCode { get; set; } // Error code for frontend handling
        public required object Details { get; set; }
    }
}
