using ECommerce.Application.Common;
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

        public async Task<(List<Product> products, int totalRecords)> GetProductsAsync(int pageNumber, int pageSize)
        {
            var query = _context.Products
                .Where(x => x.IsActive)
                .Include(x => x.Brand)
                .Include(x => x.ProductCategory)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt);

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

        public async Task<Product?> GetProductForUpdateAsync(Guid productId)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
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

        // Product Search & Filtering Module
        public async Task<(List<ProductVariant>, int totalRecords)> SearchProductsAsync(ProductSearchFilter filter)
        {
            var query = _context.ProductVariants
                .Include(x => x.Product)
                    .ThenInclude(x => x.Brand)
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductCategory)
                .Include(x => x.ProductImages)
                .AsQueryable();

            // Search by Product Name
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.Trim().ToLower().Replace(" ", "");
                query = query.Where(x => x.Product.Name.ToLower().Replace(" ", "").Contains(searchTerm));
            }

            // Filter by Category
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(x => x.Product.ProductCategoryId == filter.CategoryId.Value);
            }

            // Filter by Brand
            if (filter.BrandId.HasValue)
            {
                query = query.Where(x => x.Product.BrandId == filter.BrandId.Value);
            }

            // Filter by Size
            if (!string.IsNullOrWhiteSpace(filter.Size))
            {
                query = query.Where(x => x.Size == filter.Size);
            }

            // Filter by Color
            if (!string.IsNullOrWhiteSpace(filter.Color))
            {
                query = query.Where(x => x.Color == filter.Color);
            }

            // Filter by Minimum Price
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(x => x.Price >= filter.MinPrice);
            }

            // Filter by Maximum Price
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= filter.MaxPrice);
            }

            // Total records before pagination
            var totalRecords = await query.CountAsync();

            // Pagination
            var products = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (products, totalRecords);
        }

        //**********> BELOW API'S ARE MOVED TO ADMIN MODULE BECAUSE ONLY ADMIN CAN ADD PRODUCTS, CATEGORIES AND BRANDS
        //public async Task AddProductCategoryAsync(ProductCategory productCategory)
        //{
        //    await _context.ProductCategories.AddAsync(productCategory);
        //}

        //public async Task AddBrandAsync(Brand brand)
        //{
        //    await _context.Brands.AddAsync(brand);
        //}

        //public async Task AddProductAsync(Product product)
        //{
        //    await _context.Products.AddAsync(product);
        //}
    }
}
