using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniInventorySystem.DTO;
using MiniInventorySystem.Services;

namespace MiniInventorySystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _service;

        public CustomerController(CustomerService service)
        {
            _service = service;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _service.GetCustomerList();
            return customers.Count() == 0 ? BadRequest("Customer info not found!") : Ok(customers);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Create(CustomerDto customer)
        {
            var id = await _service.AddCustomer(customer);
            return CreatedAtRoute(null, new
            {
                success = true,
                message = "Customer info added successfully.",
                customerId = id
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, CustomerDto customer)
        {
            var updated = await _service.UpdateCustomer(id, customer);
            return updated ? CreatedAtRoute(null, new
                            {
                                success = true,
                                message = "Customer info updated successfully.",
                                customerId = id
                            })
                            : NotFound(new { success = false, message = "Customer not found." });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteCustomer(id);
            return deleted ? Ok("Deleted Successfully.") :
                            NotFound(new { success = false, message = "Customer not found." });
        }
    }
}
