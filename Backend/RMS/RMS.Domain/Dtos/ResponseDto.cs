using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Dtos
{
    public class ResponseDto<T>
    {
        public bool IsSuccess { get; set; }           // Indicates if the response is successful
        public string? Message { get; set; }           // General message (success or error)
        public string? Code { get; set; }              // Status code or error code
        public  T? Data { get; set; }                   // Payload (for success)
        public object? Details { get; set; }           // Additional error details (for failure)
    }
}
