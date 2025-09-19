namespace RMS.Application.DTOs.InventoryDTOs.InputDTOs
{
    public class CreateInventoryDto
    {
        public int ProductID { get; set; }
        public int InitialStock { get; set; }
        public int MinStockLevel { get; set; }
    }
}
