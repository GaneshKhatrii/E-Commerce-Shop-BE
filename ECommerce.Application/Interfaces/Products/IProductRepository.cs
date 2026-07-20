using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Interfaces.Products
{
    public interface IProductRepository
    {
        Task<(List<Product> products, int totalRecords)> GetProductsAsync(int pageNumber, int pageSize);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<ProductCategory?> GetProductCategoryByIdAsync(Guid productCategoryId);
        Task<Brand?> GetBrandByIdAsync(Guid brandId);
        Task<List<ProductCategory>> GetCategoriesAsync();
        Task<List<Brand>> GetBrandsAsync();
        Task SaveChangesAsync();

        // Product Search & Filtering Module
        // We are not passing pageNumber & pageSize as a separate parameters because ProductSearchFilter already includes those things
        Task<(List<ProductVariant>, int totalRecords)> SearchProductsAsync(ProductSearchFilter filter);
        Task<Product?> GetProductForUpdateAsync(Guid productId);


        //**********> BELOW API'S ARE MOVED TO ADMIN MODULE BECAUSE ONLY ADMIN CAN ADD PRODUCTS, CATEGORIES AND BRANDS
        //Task AddProductCategoryAsync(ProductCategory productCategory); 
        //Task AddBrandAsync(Brand brand); 
        //Task AddProductAsync(Product product); 
    }
}
