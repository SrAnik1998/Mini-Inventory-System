using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniInventorySystem.DTO;
using MiniInventorySystem.Models;
using MiniInventorySystem.Services;
using System.ComponentModel.DataAnnotations;

namespace MiniInventorySystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAll()
        {
            var productList = await _service.GetProductList();
            return productList.Count() == 0 ? BadRequest("No Product found!") : Ok(productList);
            
               
        } 

        [HttpPost("Add")]
        public async Task<IActionResult> Create([FromBody] ProductDto product)
        {
            if (product.Price <= 0)
            {
                throw new ValidationException("Product price should be greater then 0.");
            }
            else if (product.StockQty <= 0)
            {
                throw new ValidationException("Product quantity should be greater then 0.");
            }
            try
            {
                var id = await _service.AddProduct(product);
                return CreatedAtRoute(null, new  
                {
                    success = true,
                    message = "Product added successfully.",
                    productId = id
                });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto product)
        {
            try
            {
                var updated = await _service.UpdateProduct(id, product);
                return updated ? CreatedAtRoute(null, new
                                {
                                    success = true,
                                    message = "Product info updated successfully.",
                                    productId = id
                                })
                               : NotFound(new { success = false, message = "Product not found." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteProduct(id);
            return deleted  ? Ok("Deleted Successfully.")
                            : NotFound(new { success = false, message = "Product not found." });
        }
    }
}
