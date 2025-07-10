using System.ComponentModel.DataAnnotations;

namespace MiniInventorySystem.DTO
{
    public class CustomerDto
    {
        public string FullName { get; set; } 
        public string Phone { get; set; } 
        public string Email { get; set; }
        public int LoyaltyPoints { get; set; }
    }
}
