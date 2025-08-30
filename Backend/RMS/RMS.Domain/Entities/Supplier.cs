
using RMS.Domain.Entities;

namespace RMS.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public int Id { get; set; }
        public required string SupplierName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
