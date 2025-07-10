using MiniInventorySystem.Models;

namespace MiniInventorySystem.Repositories
{
    public interface IAuthRepositories
    {
        Task<Users?> GetUserByName(string username);
        Task<int> Register(Users user);
        //Task<bool> IsValidUserAsync(string username, string password);
    }
}
