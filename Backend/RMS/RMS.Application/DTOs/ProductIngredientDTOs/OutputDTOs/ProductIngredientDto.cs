namespace RMS.Application.DTOs.ProductIngredientDTOs.OutputDTOs
{
    public class ProductIngredientDto
    {
        public int ProductIngredientID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int IngredientID { get; set; }
        public string? IngredientName { get; set; }
        public decimal Quantity { get; set; }
        public int UnitID { get; set; }
        public string? UnitName { get; set; }
        public string? Remarks { get; set; }
        public bool Status { get; set; }
    }
}
