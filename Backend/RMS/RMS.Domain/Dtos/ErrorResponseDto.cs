using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.DTOs
{
    public class ErrorResponseDto
    {
        public string Message { get; set; } // Main error message
        public string ErrorCode { get; set; } // Error code for frontend handling
        public object Details { get; set; }
    }
}
