using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces.Products;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Infrastructure.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository repository)
        {
            _productRepository = repository;
        }

        public async Task<ApiResponse<Guid>> AddProductCategoryAsync(AddProductCategoryRequestDto request)
        {
            var productCategory = new ProductCategory()
            {
                Name = request.Name
            };

            await _productRepository.AddProductCategoryAsync(productCategory);
            await _productRepository.SaveChangesAsync();

            return new ApiResponse<Guid>
            {
                Success = true,
                StatusCode = 201,
                Message = "Product category added successfully",
                Data = productCategory.Id
            };
        }

        public async Task<ApiResponse<Guid>> AddBrandAsync(AddBrandRequestDto request)
        {
            var brand = new Brand()
            {
                Name = request.Name
            };

            await _productRepository.AddBrandAsync(brand);
            await _productRepository.SaveChangesAsync();

            return new ApiResponse<Guid>
            {
                Success = true,
                StatusCode = 201,
                Message = "Brand added successfully",
                Data = brand.Id
            };
        }

        public async Task<ApiResponse<Guid?>> AddProductAsync(AddProductRequestDto request)
        {
            // Check Category
            var productCategory = await _productRepository.GetProductCategoryByIdAsync(request.ProductCategoryId);
            if (productCategory == null)
            {
                return new ApiResponse<Guid?>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Product category not found",
                    Data = null
                };
            }

            // Check Brand
            var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);
            if (brand == null)
            {
                return new ApiResponse<Guid?>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Brand not found",
                    Data = null
                };
            }

            var product = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                ProductCategoryId = request.ProductCategoryId,
                BrandId = request.BrandId
            };

            await _productRepository.AddProductAsync(product);
            await _productRepository.SaveChangesAsync();

            return new ApiResponse<Guid?>
            {
                Success = true,
                StatusCode = 201,
                Message = "Product added successfully",
                Data = product.Id
            };
        }

        public async Task<ApiResponse<PagedResult<ProductResponseDto>>> GetProductsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var (products, totalRecords) = await _productRepository.GetProductsAsync(pageNumber, pageSize);

            var productsList = products.Select(x => new ProductResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CategoryName = x.ProductCategory.Name,
                BrandName = x.Brand.Name,
                IsActive = x.IsActive
            }).ToList();

            return new ApiResponse<PagedResult<ProductResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = productsList.Any() ? "Products retrieved successfully" : "No products found",
                Data = new PagedResult<ProductResponseDto>
                {
                    Items = productsList,
                    TotalRecords = totalRecords
                }
            };
        }

        public async Task<ApiResponse<ProductResponseDto?>> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                return new ApiResponse<ProductResponseDto?>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Product not found",
                };
            }

            var productItem = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryName = product.ProductCategory.Name,
                BrandName = product.Brand.Name,
                IsActive = product.IsActive
            };

            return new ApiResponse<ProductResponseDto?>
            {
                Success = true,
                StatusCode = 200,
                Message = "Product retrieved successfully",
                Data = productItem
            };
        }

        public async Task<ApiResponse<List<ProductCategoryResponseDto>>> GetCategoriesAsync()
        {
            var categories = await _productRepository.GetCategoriesAsync();

            var categoriesList = categories.Select(x => new ProductCategoryResponseDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return new ApiResponse<List<ProductCategoryResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = categoriesList.Any() ? "Product categories retrieved successfully" : "No product categories found",
                Data = categoriesList
            };
        }

        public async Task<ApiResponse<List<BrandResponseDto>>> GetBrandsAsync()
        {
            var brands = await _productRepository.GetBrandsAsync();

            var brandsList = brands.Select(x => new BrandResponseDto()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return new ApiResponse<List<BrandResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = brandsList.Any() ? "Brands retrieved successfully" : "No brands found",
                Data = brandsList
            };
        }

        // Product Search & Filtering Module
        public async Task<ApiResponse<PagedResult<ProductVariantResponseDto>>> SearchProductsAsync(SearchProductsRequestDto request)
        {
            // Convert DTO to repository filter model because Repository should not depend on DTOs.
            var filter = new ProductSearchFilter
            {
                SearchTerm = request.SearchTerm,
                CategoryId = request.CategoryId,
                BrandId = request.BrandId,
                Size = request.Size,
                Color = request.Color,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            };

            // Fetch filtered products from repository
            var (products, totalRecords) = await _productRepository.SearchProductsAsync(filter);

            // Map entities to DTOs
            var produtsList = products.Select(productVariant => new ProductVariantResponseDto
            {
                Id = productVariant.Id,
                ProductId = productVariant.ProductId,
                ProductName = productVariant.Product.Name,
                Size = productVariant.Size,
                Color = productVariant.Color,
                Price = productVariant.Price,
                BrandName = productVariant.Product.Brand.Name,
                CategoryName = productVariant.Product.ProductCategory.Name,
                ImageUrl = productVariant.ProductImages.FirstOrDefault(x => x.IsPrimary)?.ImageUrl ?? string.Empty
            }).ToList();

            return new ApiResponse<PagedResult<ProductVariantResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = produtsList.Any() ? "Products retrieved successfully" : "No products found",
                Data = new PagedResult<ProductVariantResponseDto>
                {
                    Items = produtsList,
                    TotalRecords = totalRecords
                }
            };
        }
    }
}
