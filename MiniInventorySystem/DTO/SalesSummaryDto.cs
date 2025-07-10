using System.ComponentModel.DataAnnotations;

namespace MiniInventorySystem.DTO
{
    public class SalesSummaryDto
    {
        public decimal TotalSales { get; set; }            
        public decimal TotalRevenue { get; set; }          
        public int NumberOfTransactions { get; set; }      
    }

}
