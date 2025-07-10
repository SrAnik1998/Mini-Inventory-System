using MiniInventorySystem.DTO;
using MiniInventorySystem.Models;
using MiniInventorySystem.Repositories;

namespace MiniInventorySystem.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Customer>> GetCustomerList()
        {
            return await _repo.GetAll();
        }

        public async Task<int> AddCustomer(CustomerDto customer)
        {
            return await _repo.Add(new Customer
            {
                FullName = customer.FullName,
                Phone = customer.Phone,
                Email = customer.Email,
                LoyaltyPoints = customer.LoyaltyPoints,
            });
        }

        public async Task<bool> UpdateCustomer(int id, CustomerDto customer)
        {
            return await _repo.Update(new Customer
            {
                CustomerId = id,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Email = customer.Email,
                LoyaltyPoints = customer.LoyaltyPoints
            });
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            return await _repo.SoftDeleteCustomer(id);
        }
    }
}
