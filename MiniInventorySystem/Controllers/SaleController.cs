using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniInventorySystem.DTO;
using MiniInventorySystem.Services;
using System.Threading.RateLimiting;

namespace MiniInventorySystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly SaleService _service;
        public static readonly SemaphoreSlim SaleSemaphore = new(3, 3);

        public SaleController(SaleService service)
        {
            _service = service;
        }

        [HttpPost("SaleTransaction")]
        public async Task<IActionResult> CreateSale([FromBody] SaleDto dto)
        {
            var entered = await SaleSemaphore.WaitAsync(0);

            if (!entered)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests,
                    "Too many concurrent sales. Please try again shortly.");
            }
            try
            {
                if (!dto.SaleDetails.Any())
                    return BadRequest("At least one product is required.");

                var (success, message, sale) = await _service.SaleTransactionAdd(dto);

                return success ? CreatedAtRoute(null, new
                                {
                                    success = true,
                                    message = message,
                                    saleInfo = new
                                    {
                                        sale.SaleId,
                                        sale.SaleDate,
                                        sale.CustomerId,
                                        sale.TotalAmount,
                                        sale.DiscountAmount,
                                        sale.VATAmount,
                                        sale.NetAmount,
                                        sale.PaidAmount,
                                        sale.DueAmount,
                                        sale.LoyaltyPointsUsed,
                                        Details = sale.SaleDetails.Select(d => new
                                        {
                                            d.ProductId,
                                            d.Quantity,
                                            d.Price
                                        })
                                    }
                                }) 
                    : BadRequest(message);
            }
            finally
            {
                SaleSemaphore.Release();
            }

        }


        [HttpGet("SalesReport")]
        public async Task<IActionResult> GetSalesReport([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from > to)
                return BadRequest("Invalid date range.");

            var summary = await _service.GetSalesData(from, to);
            return Ok(summary);
        }

    }
}
