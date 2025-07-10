using System.ComponentModel.DataAnnotations;

namespace MiniInventorySystem.DTO
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal StockQty { get; set; }
        public string Category { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
