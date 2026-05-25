
namespace RMS.Domain.Entities
{
    public class StockTransferDetail : BaseEntity
    {
        public int StockTransferDetailID { get; set; }
        public int StockTransferID { get; set; }
        public int IngredientID { get; set; }
        public decimal Quantity { get; set; }
        public int UnitID { get; set; }

        public virtual StockTransfer StockTransfer { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
