using MiniInventorySystem.Data;
using MiniInventorySystem.DTO;
using MiniInventorySystem.Models;
using MiniInventorySystem.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace MiniInventorySystem.Services
{
    public class AuthService
    {
        private readonly IAuthRepositories _repo;

        public AuthService(IAuthRepositories repo)
        {
            _repo = repo;
        }
        public async Task<Users?> GetUserByName(string username)
        {
            return await _repo.GetUserByName(username);
        }

        public async Task<int> RegisterUser(LoginDto user)
        {
            return await _repo.Register(new Users
            {
                Username = user.Username,
                PasswordHash = HashPassword(user.Password)
            });
        }

        public async Task<bool> IsValidUserAsync(LoginDto request)
        {
            var user = await GetUserByName(request.Username);
            return user != null && VerifyPassword(request.Password, user.PasswordHash);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }

        private bool VerifyPassword(string input, string hash)
        {
            return HashPassword(input) == hash;
        }
    }
}
