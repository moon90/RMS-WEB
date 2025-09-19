
namespace RMS.Application.DTOs
{
    public class CreateUnitConversionDto
    {
        public int FromUnitID { get; set; }
        public int ToUnitID { get; set; }
        public decimal ConversionFactor { get; set; }
    }
}
