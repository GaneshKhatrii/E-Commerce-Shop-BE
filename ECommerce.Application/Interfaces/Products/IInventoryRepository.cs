using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Interfaces.Products
{
    public interface IInventoryRepository
    {
        Task AddInventoryAsync(Inventory inventory);
        Task<Inventory?> GetInventoryByVariantIdAsync(Guid productVariantId);
        Task<(List<Inventory> inventories, int totalRecords)> GetInventoriesAsync(int pageNumber, int pageSize);
        Task<ProductVariant?> GetProductVariantByIdAsync(Guid productVariantId);
        Task SaveChangesAsync();
    }
}
