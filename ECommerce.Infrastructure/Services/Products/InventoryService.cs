using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces.Products;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Infrastructure.Services.Products
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<ApiResponse<string>> AddInventoryAsync(AddInventoryRequestDto request)
        {
            // Check variant exists
            var productVariant = await _inventoryRepository.GetProductVariantByIdAsync(request.ProductVariantId);

            if (productVariant == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Product variant not found."
                };
            }

            // Prevent duplicate inventory, One product variant should have only one inventory record
            var existingInventory = await _inventoryRepository.GetInventoryByVariantIdAsync(request.ProductVariantId);

            if (existingInventory != null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Inventory for this product variant already exists."
                };
            }

            var inventory = new Inventory
            {
                ProductVariantId = request.ProductVariantId,
                AvailableStock = request.AvailableStock
            };

            await _inventoryRepository.AddInventoryAsync(inventory);
            await _inventoryRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = 201,
                Message = "Inventory created successfully"
            };
        }

        public async Task<ApiResponse<String>> UpdateStockAsync(Guid productVariantId, UpdateInventoryStockRequestDto request)
        {
            var inventory = await _inventoryRepository.GetInventoryByVariantIdAsync(productVariantId);

            if (inventory == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Inventory not found."
                };
            }

            inventory.AvailableStock = request.AvailableStock;
            await _inventoryRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = 200,
                Message = "Stock updated successfully"
            };
        }

        public async Task<ApiResponse<InventoryResponseDto?>> GetInventoryByVariantIdAsync(Guid productVariantId)
        {
            var inventory = await _inventoryRepository.GetInventoryByVariantIdAsync(productVariantId);

            if (inventory == null)
            {
                return new ApiResponse<InventoryResponseDto?>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Inventory not found."
                };
            }

            if (IsOutOfStock(inventory))
            {
                return new ApiResponse<InventoryResponseDto?>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Product is out of stock"
                };
            }
            var inventoryData = new InventoryResponseDto
            {
                Id = inventory.Id,
                ProductVariantId = inventory.ProductVariantId,
                ProductName = inventory.ProductVariant.Product.Name,
                Size = inventory.ProductVariant.Size,
                Color = inventory.ProductVariant.Color,
                Price = inventory.ProductVariant.Price,
                AvailableStock = inventory.AvailableStock,
            };

            return new ApiResponse<InventoryResponseDto?>
            {
                Success = true,
                StatusCode = 200,
                Message = "Inventory retrieved successfully",
                Data = inventoryData
            };
        }

        public async Task<ApiResponse<PagedResult<InventoryResponseDto>>> GetInventoriesAsync(int pageNumber, int pageSize)
        {
            // Pagination protection
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var (inventories, totalRecords) = await _inventoryRepository.GetInventoriesAsync(pageNumber, pageSize);

            var inventoriesList = inventories.Select(inventory => new InventoryResponseDto
            {
                Id = inventory.Id,
                ProductVariantId = inventory.ProductVariantId,
                ProductName = inventory.ProductVariant.Product.Name,
                Size = inventory.ProductVariant.Size,
                Color = inventory.ProductVariant.Color,
                Price = inventory.ProductVariant.Price,
                AvailableStock = inventory.AvailableStock,
            });

            return new ApiResponse<PagedResult<InventoryResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = inventoriesList.Any() ? "Inventory retrieved successfully" : "Inventories not found",
                Data = new PagedResult<InventoryResponseDto>
                {
                    Items = inventoriesList,
                    TotalRecords = totalRecords,
                }
            };
        }

        private bool IsOutOfStock(Inventory inventory)
        {
            return inventory.AvailableStock <= 0;
        }
    }
}
