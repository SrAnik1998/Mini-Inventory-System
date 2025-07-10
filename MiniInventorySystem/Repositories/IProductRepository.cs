using MiniInventorySystem.Models;

namespace MiniInventorySystem.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(int id);
        Task<bool> IsBarcodeExists(string barcode, int? excludeProductId = null);
        Task<int> Add(Product product);
        Task<bool> Update(Product product);
        Task<bool> SoftDeleteProduct(int id);

        Task<bool> DecreaseStockAsync(int productId, decimal quantity);
    }
}
