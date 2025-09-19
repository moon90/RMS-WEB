namespace RMS.Application.DTOs.CustomerDTOs.InputDTOs
{
    public class UpdateCustomerDto
    {
        public int CustomerID { get; set; }

        public required string CustomerName { get; set; }

        public string? CustomerPhone { get; set; }

        public string? CustomerEmail { get; set; }

        public string? Address { get; set; }

        public string? DriverName { get; set; }

        public bool Status { get; set; }
    }
}
