using Dapper;
using MiniInventorySystem.Data;
using MiniInventorySystem.Models;

namespace MiniInventorySystem.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbContext _context;

        public CustomerRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            var query = "SELECT * FROM Customer WHERE IsDeleted = 0";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Customer>(query);
        }

        public async Task<Customer?> GetById(int id)
        {
            var query = "SELECT * FROM Customer WHERE CustomerId = @Id AND IsDeleted = 0";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
        }

        public async Task<int> Add(Customer customer)
        {
            var query = @"INSERT INTO Customer (FullName, Phone, Email, LoyaltyPoints)
                      VALUES (@FullName, @Phone, @Email, @LoyaltyPoints);
                      SELECT CAST(SCOPE_IDENTITY() as int);";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, customer);
        }

        public async Task<bool> Update(Customer customer)
        {
            var query = @"UPDATE Customer SET FullName = @FullName, Phone = @Phone,
                      Email = @Email, LoyaltyPoints = @LoyaltyPoints WHERE CustomerId = @CustomerId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, customer);
            return rows > 0;
        }

        public async Task<bool> SoftDeleteCustomer(int customerId)
        {
            var query = "UPDATE Customer SET IsDeleted = 1 WHERE CustomerId = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = customerId });
            return affectedRows > 0;
        }

        public async Task<bool> UpdateLoyaltyPoints(int customerId, int updatedPoints)
        {
            var sql = "UPDATE Customer SET LoyaltyPoints = @updatedPoints WHERE CustomerId = @customerId";
            using var connection = _context.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new { customerId, updatedPoints });
            return affected > 0;
        }
    }

}
