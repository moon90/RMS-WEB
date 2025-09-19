
using RMS.Domain.Models.BaseModels;

namespace RMS.Domain.Entities
{
    public class UnitConversion : BaseEntity
    {
        public int UnitConversionID { get; set; }
        public int FromUnitID { get; set; }
        public int ToUnitID { get; set; }
        public decimal ConversionFactor { get; set; }

        // Navigation properties
        public Unit? FromUnit { get; set; }
        public Unit? ToUnit { get; set; }
    }
}
