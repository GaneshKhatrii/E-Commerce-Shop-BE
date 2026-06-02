using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Interfaces.Products
{
    public interface IproductVariantRepository
    {
        Task AddProductVariantAsync(ProductVariant productVariant);
        Task<List<ProductVariant>> GetProductVariantsAsync();
        Task<List<ProductVariant>> GetVariantsByProductIdAsync(Guid productId);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task SaveChangesAsync();
    }
}
