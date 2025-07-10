namespace MiniInventorySystem.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public int? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public int LoyaltyPointsUsed { get; set; }
        public List<SaleDetail> SaleDetails { get; set; } = new();
    }
}
