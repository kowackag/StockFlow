namespace StockFlow.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public required string Status { get; init; }
    }
}
