using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces.Products;
using ECommerce.Domain.Entities.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddInventory(AddInventoryRequestDto request)
        {
            var result = await _inventoryService.AddInventoryAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{productVariantId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStock([FromRoute] Guid productVariantId, UpdateInventoryStockRequestDto request)
        {
            var result = await _inventoryService.UpdateStockAsync(productVariantId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{productVariantId}")]
        public async Task<IActionResult> GetInventoryByVariantId([FromRoute] Guid productVariantId)
        {
            var result = await _inventoryService.GetInventoryByVariantIdAsync(productVariantId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetInventories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _inventoryService.GetInventoriesAsync(pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }
    }
}
