using Dapper;
using MiniInventorySystem.Data;
using MiniInventorySystem.Models;
using System.Security.Cryptography;
using System.Text;

namespace MiniInventorySystem.Repositories
{
    public class AuthRepositories : IAuthRepositories
    {
        private readonly DbContext _context;

        public AuthRepositories(DbContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetUserByName(string username)
        {
            var query = "Select * From Users Where Username = @UserName";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Users>(query, new { @UserName = username });
        }

        public async Task<int> Register(Users user)
        {
            var query = @"INSERT INTO Users (Username, PasswordHash)
                          VALUES (@Username, @PasswordHash)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, user);
        }
    }
}
