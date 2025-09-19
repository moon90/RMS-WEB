namespace RMS.Application.DTOs
{
    public class TokenValidationResultDto
    {
        public bool IsValid { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Message { get; set; }
        public object? Details { get; set; }
    }
}
