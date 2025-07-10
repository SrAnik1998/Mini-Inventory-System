using System.ComponentModel.DataAnnotations;

namespace MiniInventorySystem.DTO
{
    public class SaleDto
    {
        public int? CustomerId { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VATPercent { get; set; }
        public int LoyaltyPointsUsed { get; set; }
        public List<SaleDetailDto> SaleDetails { get; set; } = new();
    }
}
