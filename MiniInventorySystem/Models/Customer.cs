using System.ComponentModel.DataAnnotations;

namespace MiniInventorySystem.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } 
        public string Phone { get; set; } 
        public string Email { get; set; } 
        public int LoyaltyPoints { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
