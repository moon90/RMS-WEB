namespace RMS.Domain.Entities
{
    public class Category : BaseEntity
    {
        public int CategoryID { get; set; }
        public required string CategoryName { get; set; }
    }
}
