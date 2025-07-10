using MiniInventorySystem.DTO;
using MiniInventorySystem.Models;

namespace MiniInventorySystem.Repositories
{
    public interface ISaleRepository
    {
        Task<int> AddSaleInfo(Sale sale);

        Task<SalesSummaryDto?> GetSalesData(DateTime fromDate, DateTime toDate);

    }
}
