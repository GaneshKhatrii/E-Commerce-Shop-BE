using ECommerce.Application.Interfaces.Products;
using ECommerce.Domain.Entities.Products;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Products
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProductImageAsync(ProductImage productImage)
        {
            await _context.ProductImages.AddAsync(productImage);
        }

        public async Task<List<ProductImage>> GetImagesByVariantIdAsync(Guid productVariantId)
        {
            return await _context.ProductImages
                .Where(image => image.ProductVariantId == productVariantId)
                .OrderBy(image => image.DisplayOrder)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductVariant?> GetProductVariantByIdAsync(Guid productVariantId)
        {
            return await _context.ProductVariants
                .AsNoTracking()
                .FirstOrDefaultAsync(pv => pv.Id == productVariantId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
