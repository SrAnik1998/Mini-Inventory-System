using Azure.Core;
using MiniInventorySystem.DTO;
using MiniInventorySystem.Models;
using MiniInventorySystem.Repositories;

namespace MiniInventorySystem.Services
{
    public class SaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public SaleService(ISaleRepository saleRepository, IProductRepository productRepository, ICustomerRepository customerRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;

        }

        public async Task<(bool success, string message, Sale? sale)> SaleTransactionAdd(SaleDto request)
        {
            await Task.Delay(3000);
            foreach (var detail in request.SaleDetails)
            {
                // Validation: 1. Check if product exists
                var product = await _productRepository.GetById(detail.ProductId);
                if (product == null)
                {
                    return (false, $"Product with ID {detail.ProductId} does not exist.", null);
                }

                // Validation: 2. Check stock quantity
                if (product.StockQty < detail.Quantity)
                {
                    return (false, $"Not enough stock for product {product.Name}. Available: {product.StockQty}", null);
                }
            }
            Customer customer = null;

            //Load and validate customer if present
            if (request.CustomerId.HasValue)
            {
                customer = await _customerRepository.GetById(request.CustomerId.Value);
                if (customer == null)
                    return (false, "Customer not found.", null);

                if (request.LoyaltyPointsUsed > customer.LoyaltyPoints)
                    return (false, "Insufficient loyalty points.", null);
            }

            if (request.PaidAmount < 0)
                return (false, "Paid amount can not be 0.", null);

            var sale = new Sale
            {
                SaleDate = DateTime.Now,
                CustomerId = request.CustomerId,
                PaidAmount = request.PaidAmount,
                LoyaltyPointsUsed = request.LoyaltyPointsUsed,
                SaleDetails = request.SaleDetails.Select(d => new SaleDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    Price = d.Price
                }).ToList()
            };

            // Calculate total from details
            sale.TotalAmount = request.SaleDetails.Sum(x => x.Quantity * x.Price);

            // Apply Discount + Loyalty points as discount
            sale.DiscountAmount = customer is not null ? 
                                    request.DiscountAmount + request.LoyaltyPointsUsed 
                                    : request.DiscountAmount;

            // Calculate VAT
            var taxableAmount = sale.TotalAmount - sale.DiscountAmount;
            sale.VATAmount = taxableAmount * (request.VATPercent / 100);

            // Final Net
            sale.NetAmount = taxableAmount + sale.VATAmount;

            // Due
            sale.DueAmount = sale.NetAmount - sale.PaidAmount;

            var saleId = await _saleRepository.AddSaleInfo(sale);
            if (saleId <= 0)
                return (false, "Failed to save sale.", null);

            sale.SaleId = saleId;

            // Now Loyalty point update
            if (customer != null)
            {
                // Minus used points
                customer.LoyaltyPoints -= request.LoyaltyPointsUsed;

                // Earn points from NetAmount (e.g. 1 per 100)
                var pointsEarned = (int)(sale.NetAmount / 100);
                customer.LoyaltyPoints += pointsEarned;

                await _customerRepository.UpdateLoyaltyPoints(customer.CustomerId, customer.LoyaltyPoints);
            }


            // Now Reduce product stock after successful sale
            foreach (var detail in request.SaleDetails)
            {
                await _productRepository.DecreaseStockAsync(detail.ProductId, detail.Quantity);
            }

            return (true, "Transaction created successfully", sale);
        }

        public async Task<SalesSummaryDto?> GetSalesData(DateTime fromDate, DateTime toDate)
        {
            return await _saleRepository.GetSalesData(fromDate, toDate);
        }

    }

}
