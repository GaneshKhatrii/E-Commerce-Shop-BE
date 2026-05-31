using ECommerce.Application.Interfaces.Products;
using ECommerce.Domain.Entities.Products;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Products
{
    public class ProductVariantRepository : IproductVariantRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductVariantRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddProductVariantAsync(ProductVariant productVariant)
        {
            await _context.ProductVariants.AddAsync(productVariant);
        }

        public async Task<(List<ProductVariant> variants, int totalRecords)> GetProductVariantsAsync(int pageNumber, int pageSize)
        {
            var query = _context.ProductVariants.Include(x => x.Product).AsNoTracking();
            var totalRecords = await query.CountAsync();

            var variants = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (variants, totalRecords);
        }

        public async Task<List<ProductVariant>> GetVariantsByProductIdAsync(Guid productId)
        {
            return await _context.ProductVariants
                .Include(pv => pv.Product)
                .Where(x => x.ProductId == productId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
