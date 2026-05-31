using ECommerce.Application.Interfaces.Products;
using ECommerce.Domain.Entities.Products;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProductCategoryAsync(ProductCategory productCategory)
        {
            await _context.ProductCategories.AddAsync(productCategory);
        }

        public async Task AddBrandAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task<(List<Product> products, int totalRecords)> GetProductsAsync(int pageNumber, int pageSize)
        {
            var query = _context.Products
                .Include(x => x.ProductCategory)
                .Include(x => x.Brand)
                .AsNoTracking();

            var totalRecords = await query.CountAsync();
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalRecords);
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products
                .Include(x => x.ProductCategory)
                .Include(x => x.Brand)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<List<ProductCategory>> GetCategoriesAsync()
        {
            return await _context.ProductCategories.AsNoTracking().ToListAsync();
        }
        public async Task<ProductCategory?> GetProductCategoryByIdAsync(Guid productCategoryId)
        {
            return await _context.ProductCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == productCategoryId);
        }

        public async Task<List<Brand>> GetBrandsAsync()
        {
            return await _context.Brands.AsNoTracking().ToListAsync();
        }
        public async Task<Brand?> GetBrandByIdAsync(Guid brandId)
        {
            return await _context.Brands.AsNoTracking().FirstOrDefaultAsync(x => x.Id == brandId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
