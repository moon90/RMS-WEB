
namespace RMS.Application.DTOs
{
    public class UnitConversionDto
    {
        public int UnitConversionID { get; set; }
        public int FromUnitID { get; set; }
        public string? FromUnitName { get; set; }
        public int ToUnitID { get; set; }
        public string? ToUnitName { get; set; }
        public decimal ConversionFactor { get; set; }
    }
}
