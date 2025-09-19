namespace RMS.Application.DTOs.ProductIngredientDTOs.InputDTOs
{
    public class UpdateProductIngredientDto
    {
        public int ProductIngredientID { get; set; }
        public int ProductID { get; set; }
        public int IngredientID { get; set; }
        public decimal Quantity { get; set; }
        public int UnitID { get; set; }
        public string? Remarks { get; set; }
        public bool Status { get; set; }
    }
}
