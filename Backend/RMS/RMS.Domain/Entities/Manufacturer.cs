
using RMS.Domain.Entities;

namespace RMS.Domain.Entities
{
    public class Manufacturer : BaseEntity
    {
        public int Id { get; set; }
        public required string ManufacturerName { get; set; }
    }
}
