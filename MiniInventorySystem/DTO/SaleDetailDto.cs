using System.ComponentModel.DataAnnotations;

namespace MiniInventorySystem.DTO
{
    public class SaleDetailDto
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
