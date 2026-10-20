using StockFlow.Application.DTOs;

namespace StockFlow.Application.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductDto>> GetAllAsync();
    }
}
