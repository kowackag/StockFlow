using StockFlow.Domain.Model.Products;

namespace StockFlow.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetAllAsync();
    }
}
