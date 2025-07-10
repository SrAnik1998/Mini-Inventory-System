using MiniInventorySystem.DTO;
using MiniInventorySystem.Models;
using MiniInventorySystem.Repositories;

namespace MiniInventorySystem.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Product>> GetProductList()
        {
            return await _repo.GetAll();
        }

        public async Task<int> AddProduct(ProductDto product)
        {
            var barcodeExists = await _repo.IsBarcodeExists(product.Barcode);
            if (barcodeExists)
                throw new ApplicationException("A product with this barcode already exists.");


            return await _repo.Add(new Product
            {
                Name = product.Name,
                Barcode = product.Barcode,
                Price = product.Price,
                StockQty = product.StockQty,
                Category = product.Category,
                Status = product.Status,
            });
        }

        public async Task<bool> UpdateProduct(int id, ProductDto product)
        {
            var barcodeExists = await _repo.IsBarcodeExists(product.Barcode, id);
            if (barcodeExists)
                throw new ApplicationException("A different product with this barcode already exists.");

            return await _repo.Update(new Product
            {
                ProductId = id,
                Name = product.Name,
                Barcode = product.Barcode,
                Price = product.Price,
                StockQty = product.StockQty,
                Category = product.Category,
                Status = product.Status,
            });
        }

        public async Task<bool> DeleteProduct(int id)
        {
            return await _repo.SoftDeleteProduct(id);
        }
    }
}
