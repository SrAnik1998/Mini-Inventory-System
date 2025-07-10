using Dapper;
using MiniInventorySystem.Data;
using MiniInventorySystem.Models;

namespace MiniInventorySystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _context;

        public ProductRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var query = "SELECT * FROM Product WHERE IsDeleted = 0";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(query);
        }

        public async Task<Product?> GetById(int id)
        {
            var query = "SELECT * FROM Product WHERE ProductId = @Id AND IsDeleted = 0";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Product>(query, new { Id = id });
        }
        public async Task<bool> IsBarcodeExists(string barcode, int? excludeProductId = null)
        {
            var query = "SELECT COUNT(1) FROM Product WHERE Barcode = @Barcode";

            if (excludeProductId.HasValue)
                query += " AND ProductId != @ExcludeId";

            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(query, new { Barcode = barcode, ExcludeId = excludeProductId });
            return count > 0;
        }
        public async Task<int> Add(Product product)
        {
            var query = @"INSERT INTO Product (Name, Barcode, Price, StockQty, Category, Status)
                      VALUES (@Name, @Barcode, @Price, @StockQty, @Category, @Status);";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, product);
        }

        public async Task<bool> Update(Product product)
        {
            var query = @"UPDATE Product SET Name = @Name, Barcode = @Barcode,
                      Price = @Price, StockQty = @StockQty, Category = @Category, Status = @Status
                      WHERE ProductId = @ProductId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, product);
            return rows > 0;
        }

        public async Task<bool> SoftDeleteProduct(int productId)
        {
            var query = "UPDATE Product SET IsDeleted = 1 WHERE ProductId = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = productId });
            return affectedRows > 0;
        }

        public async Task<bool> DecreaseStockAsync(int productId, decimal quantity)
        {
            var sql = "UPDATE Product SET StockQty = StockQty - @Qty WHERE ProductId = @ProductId AND StockQty >= @Qty";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(sql, new { Qty = quantity, ProductId = productId });
            return rows > 0;
        }
    }
}
