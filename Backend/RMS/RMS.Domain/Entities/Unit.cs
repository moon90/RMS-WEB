using RMS.Domain.Entities;

namespace RMS.Domain.Entities
{
    public class Unit : BaseEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ShortCode { get; set; }
    }
}
