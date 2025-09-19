
namespace RMS.Domain.Entities
{
    public class ProductIngredient : BaseEntity
    {
        public int ProductIngredientID { get; set; }
        public int ProductID { get; set; }
        public int IngredientID { get; set; }
        public decimal Quantity { get; set; }
        public int UnitID { get; set; }
        public string? Remarks { get; set; }

        // Navigation properties
        public Product? Product { get; set; }
        public Ingredient? Ingredient { get; set; }
        public Unit? Unit { get; set; }
    }
}
