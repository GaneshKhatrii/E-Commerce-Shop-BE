using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.DTOs.Admin.Products;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.DTOs.User;
using ECommerce.Application.Interfaces.Admin;
using ECommerce.Application.Interfaces.Products;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.Enums;

namespace ECommerce.Infrastructure.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IProductRepository _productRepository;
        public AdminService(IAdminRepository adminRepository, IProductRepository productRepository)
        {
            _adminRepository = adminRepository;
            _productRepository = productRepository;
        }

        public async Task<ApiResponse<DashboardStatsResponseDto>> GetDashboardStatsAsync()
        {
            var dashboardstats = new DashboardStatsResponseDto
            {
                TotalUsers = await _adminRepository.GetTotalUsersAsync(),
                TotalProducts = await _adminRepository.GetTotalProductAsync(),
                TotalOrders = await _adminRepository.GetTotalOrdersAsync(),
                TotalRevenue = await _adminRepository.GetTotalRevenueAsync(),
            };

            return new ApiResponse<DashboardStatsResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Dashboard data retrieved successfully",
                Data = dashboardstats
            };
        }

        public async Task<ApiResponse<PagedResult<UserProfileResponseDto>>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            var (users, totalRecords) = await _adminRepository.GetAllUsersAsync(pageNumber, pageSize);

            var usersList = users.Select(user => new UserProfileResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailVerified = user.IsEmailVerified,
            }).ToList();

            return new ApiResponse<PagedResult<UserProfileResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Users retrieved succeessfully",
                Data = new PagedResult<UserProfileResponseDto>
                {
                    Items = usersList,
                    TotalRecords = totalRecords
                }
            };
        }

        public async Task<ApiResponse<PagedResult<ProductResponseDto>>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var (products, totalRecords) = await _adminRepository.GetProductsAsync(pageNumber, pageSize);

            var productsList = products.Select(product => new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryName = product.ProductCategory.Name,
                BrandName = product.Brand.Name,
                IsActive = product.IsActive,
            }).ToList();

            return new ApiResponse<PagedResult<ProductResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Products  retrieved successfully",
                Data = new PagedResult<ProductResponseDto>
                {
                    Items = productsList,
                    TotalRecords = totalRecords
                }
            };
        }

        public async Task<ApiResponse<PagedResult<AdminOrderListResponseDto>>> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            var (orders, totalRecords) = await _adminRepository.GetAllOrdersAsync(pageNumber, pageSize);

            var ordersList = orders.Select(order => new AdminOrderListResponseDto
            {
                OrderId = order.Id,
                CustomerName = order.FullName,
                CustomerEmail = order.Email,
                PhoneNumber = order.PhoneNumber,
                TotalAmount = order.TotalAmount,
                StatusId = (int)order.Status,
                StatusName = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
            }).ToList();

            return new ApiResponse<PagedResult<AdminOrderListResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Orders retrieved successfully",
                Data = new PagedResult<AdminOrderListResponseDto>
                {
                    Items = ordersList,
                    TotalRecords = totalRecords
                }
            };
        }

        public async Task<ApiResponse<AdminOrderDetailsResponseDto?>> GetOrderDetailsByIdAsync(Guid orderId)
        {
            var order = await _adminRepository.GetOrderDetailsByIdAsync(orderId);

            if (order == null)
            {
                return new ApiResponse<AdminOrderDetailsResponseDto?>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Order not found"
                };
            }

            var orderItems = order.OrderItems.Select(item => new OrderItemResponseDto
            {
                ProductVariantId = item.ProductVariantId,
                ProductName = item.ProductVariant.Product.Name,
                Size = item.ProductVariant.Size,
                Color = item.ProductVariant.Color,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                SubTotal = item.UnitPrice * item.Quantity,
                ImageUrl = item.ImageUrl
            }).ToList();

            var orderDetails = new AdminOrderDetailsResponseDto
            {
                OrderId = order.Id,
                CustomerName = order.FullName,
                CustomerEmail = order.Email,
                PhoneNumber = order.PhoneNumber,
                AddAddressLine1 = order.AddressLine1,
                AddressLine2 = order.AddressLine2,
                City = order.City,
                State = order.State,
                Country = order.Country,
                PostalCode = order.PostalCode,
                TotalAmount = order.TotalAmount,
                StatusId = (int)order.Status,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                OrderItems = orderItems
            };

            return new ApiResponse<AdminOrderDetailsResponseDto?>
            {
                Success = true,
                StatusCode = 200,
                Message = "Order details retrieved successfully",
                Data = orderDetails
            };
        }

        // Order Status Management Module
        public async Task<ApiResponse<string>> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequestDto request)
        {
            var order = await _adminRepository.GetOrderByIdForUpdateAsync(orderId);

            if (order == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Order not found"
                };
            }

            // Validate allowed status transitions
            var IsValidTransition =
                (order.Status == OrderStatus.Pending &&
                (request.Status == OrderStatus.Confirmed || request.Status == OrderStatus.Cancelled))

                ||

                (order.Status == OrderStatus.Confirmed &&
                (request.Status == OrderStatus.Shipped || request.Status == OrderStatus.Delivered));

            // Update order status
            order.Status = request.Status;

            // Save Changes
            await _adminRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = false,
                StatusCode = 200,
                Message = "Order status changed"
            };
        }

        // Product Module

        // ******> THESE METHODS ARE MOVED FROM PRODUCT MODULE TO ADMIN MODULE BECAUSE ONLY ADMIN CAN ADD PRODUCT, CATEGORY AND BRAND
        public async Task<ApiResponse<Guid>> AddProductCategoryAsync(AddProductCategoryRequestDto request)
        {
            var productCategory = new ProductCategory()
            {
                Name = request.Name
            };

            await _adminRepository.AddProductCategoryAsync(productCategory);
            await _adminRepository.SaveChangesAsync();

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

            await _adminRepository.AddBrandAsync(brand);
            await _adminRepository.SaveChangesAsync();

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

            await _adminRepository.AddProductAsync(product);
            await _adminRepository.SaveChangesAsync();

            return new ApiResponse<Guid?>
            {
                Success = true,
                StatusCode = 201,
                Message = "Product added successfully",
                Data = product.Id
            };
        }

        // Both admin and product module has this method but the data they return are different so we have to keep this method in both modules
        public async Task<ApiResponse<AdminProductResponseDto?>> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                return new ApiResponse<AdminProductResponseDto?>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Product not found",
                };
            }

            var productItem = new AdminProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                productCategoryId = product.ProductCategory.Id,
                brandId = product.Brand.Id,
                IsActive = product.IsActive
            };

            return new ApiResponse<AdminProductResponseDto?>
            {
                Success = true,
                StatusCode = 200,
                Message = "Product retrieved successfully",
                Data = productItem
            };
        }

        public async Task<ApiResponse<string>> UpdateProductAsync(Guid productId, UpdateProductRequestDto request)
        {
            var product = await _productRepository.GetProductForUpdateAsync(productId);
            if (product == null) 
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Product not found"
                };
            }

            var category = await _productRepository.GetProductCategoryByIdAsync(request.ProductCategoryId);
            if (category == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Product category not found"
                };
            }

            var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);
            if(brand == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Brand not found"
                };
            }

            // Update Product with new data requested by user
            product.Name = request.Name;
            product.Description = request.Description;
            product.ProductCategoryId = request.ProductCategoryId;
            product.BrandId = request.BrandId;

            // Save changes
            await _productRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = 200,
                Message = "Product updated successfully"
            };
        }

        public async Task<ApiResponse<string>> UpdateProductStatusAsync(Guid productId, UpdateProductStatusDto request)
        {
            var product = await _productRepository.GetProductForUpdateAsync(productId);

            if(product == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Product not found"
                };
            }

            // Update product status
            product.IsActive = request.IsActive;

            await _productRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = 200,
                Message = request.IsActive ? "Product activated successfully" : "Product deactivated succesfully"
            };
        }

        //public async Task<ApiResponse<string>> UpdateProductVariantAsync(Guid productId, UpdateProductVariantRequestDto request)
        //{

        //}
    }
}
