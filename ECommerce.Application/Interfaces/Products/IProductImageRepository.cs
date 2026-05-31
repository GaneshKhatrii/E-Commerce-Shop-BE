using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Interfaces.Products
{
    public interface IProductImageRepository
    {
        Task AddProductImageAsync(ProductImage productImage);
        Task<List<ProductImage>> GetImagesByVariantIdAsync(Guid productVariantId);
        Task<ProductVariant?> GetProductVariantByIdAsync(Guid productVariantId);
        Task SaveChangesAsync();
    }
}
