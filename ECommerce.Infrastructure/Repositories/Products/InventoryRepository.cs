using ECommerce.Application.Interfaces.Products;
using ECommerce.Domain.Entities.Products;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Products
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;
        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
        }

        public async Task<Inventory?> GetInventoryByVariantIdAsync(Guid productVariantId)
        {
            return await _context.Inventories
                .Include(pv => pv.ProductVariant)
                    .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(i => i.ProductVariantId == productVariantId);
        }

        public async Task<(List<Inventory> inventories, int totalRecords)> GetInventoriesAsync(int pageNumber, int pageSize)
        {
            var query = _context.Inventories
                .Include(pv => pv.ProductVariant)
                    .ThenInclude(p => p.Product)
                .AsNoTracking();

            var totalRecords = await query.CountAsync();
            var inventories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (inventories, totalRecords);
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
