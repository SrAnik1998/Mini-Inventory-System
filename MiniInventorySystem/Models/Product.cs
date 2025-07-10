namespace MiniInventorySystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal StockQty { get; set; }
        public string Category { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
