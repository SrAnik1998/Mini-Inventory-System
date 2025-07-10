using MiniInventorySystem.Models;

namespace MiniInventorySystem.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer?> GetById(int id);
        Task<int> Add(Customer customer);
        Task<bool> Update(Customer customer);
        Task<bool> SoftDeleteCustomer(int id);
        Task<bool> UpdateLoyaltyPoints(int customerId, int updatedPoints);
    }
}
