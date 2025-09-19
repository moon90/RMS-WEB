namespace RMS.Application.DTOs
{
    public class ErrorResponseDto
    {
        public required string Message { get; set; } // Main error message
        public required string ErrorCode { get; set; } // Error code for frontend handling
        public required object Details { get; set; }
    }
}
