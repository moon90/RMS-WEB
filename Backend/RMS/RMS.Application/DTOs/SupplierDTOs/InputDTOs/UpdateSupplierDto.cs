
namespace RMS.Application.DTOs.SupplierDTOs.InputDTOs
{
    public class UpdateSupplierDto
    {
        public int Id { get; set; }
        public required string SupplierName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool Status { get; set; }
    }
}
