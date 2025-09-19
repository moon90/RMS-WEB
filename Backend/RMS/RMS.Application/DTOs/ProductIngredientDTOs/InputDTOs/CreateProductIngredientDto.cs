namespace RMS.Application.DTOs.ProductIngredientDTOs.InputDTOs
{
    public class CreateProductIngredientDto
    {
        public int ProductID { get; set; }
        public int IngredientID { get; set; }
        public decimal Quantity { get; set; }
        public int UnitID { get; set; }
        public string? Remarks { get; set; }
    }
}
