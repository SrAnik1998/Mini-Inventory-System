using System.ComponentModel.DataAnnotations;

namespace MiniInventorySystem.Models
{
    public class SaleDetail
    {
        public int SaleDetailId { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
