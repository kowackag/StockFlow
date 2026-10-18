namespace StockFlow.Domain.Model.Products
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public PublicationStatus Status { get; set; }
    }
}
