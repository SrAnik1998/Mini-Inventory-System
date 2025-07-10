using Dapper;
using MiniInventorySystem.Data;
using MiniInventorySystem.DTO;
using MiniInventorySystem.Models;
using System.Data;

namespace MiniInventorySystem.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DbContext _context;

        public SaleRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<int> AddSaleInfo(Sale sale)
        {
            var query = @"
                        INSERT INTO Sale (SaleDate, CustomerId, TotalAmount, DiscountAmount, VATAmount, NetAmount, PaidAmount, DueAmount, LoyaltyPointsUsed)
                        VALUES (@SaleDate, @CustomerId, @TotalAmount, @DiscountAmount, @VATAmount, @NetAmount, @PaidAmount, @DueAmount, @LoyaltyPointsUsed);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var saleId = await connection.QuerySingleAsync<int>(query, sale, transaction);

                foreach (var detail in sale.SaleDetails)
                {
                    detail.SaleId = saleId;
                    var detailQuery = @"INSERT INTO SaleDetail (SaleId, ProductId, Quantity, Price)
                                    VALUES (@SaleId, @ProductId, @Quantity, @Price);";

                    await connection.ExecuteAsync(detailQuery, detail, transaction);
                }

                transaction.Commit();
                return saleId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<SalesSummaryDto?> GetSalesData(DateTime fromDate, DateTime toDate)
        {
            var param = new DynamicParameters();
            using var connection = _context.CreateConnection();
            var query = @"
                        WITH TmpSalesCTE AS (
                            SELECT 
                                s.SaleId,
                                s.NetAmount,
                                SUM(sd.Quantity) AS TotalQuantity
                            FROM 
                                Sale s
                                LEFT JOIN SaleDetail sd ON s.SaleId = sd.SaleId
                            WHERE
                                CAST(s.SaleDate as date) BETWEEN CAST(@FromDate as date) AND CAST(@ToDate as date)
                            GROUP BY 
                                s.SaleId, s.NetAmount
                        )
                        SELECT 
                            ISNULL(SUM(TotalQuantity), 0) AS TotalSales,
                            ISNULL(SUM(NetAmount), 0) AS TotalRevenue,
                            COUNT(*) AS NumberOfTransactions
                        FROM 
                            TmpSalesCTE";

            param.Add("@FromDate", fromDate, DbType.DateTime, ParameterDirection.Input);
            param.Add("@ToDate", toDate, DbType.DateTime, ParameterDirection.Input);
            return await connection.QueryFirstOrDefaultAsync<SalesSummaryDto>(query, param, commandTimeout: 0);
        }

    }

}
