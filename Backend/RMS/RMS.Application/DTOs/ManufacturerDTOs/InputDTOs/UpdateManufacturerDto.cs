
namespace RMS.Application.DTOs.ManufacturerDTOs.InputDTOs
{
    public class UpdateManufacturerDto
    {
        public int Id { get; set; }
        public required string ManufacturerName { get; set; }
        public bool Status { get; set; }
    }
}
