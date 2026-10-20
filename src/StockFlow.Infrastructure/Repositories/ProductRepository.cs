using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Model.Products;

namespace StockFlow.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StockFlowDbContext _context;
        public ProductRepository(StockFlowDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }
    }
}
