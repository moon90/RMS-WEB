namespace RMS.Application.DTOs
{
    public class ValidationDetailDto
    {
        public required string PropertyName { get; set; }
        public required string ErrorMessage { get; set; }
    }
}
