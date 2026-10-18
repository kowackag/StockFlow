using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Model.Products;
namespace StockFlow.Infrastructure
{
    public class StockFlowDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public StockFlowDbContext(DbContextOptions<StockFlowDbContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).IsRequired().HasConversion<string>();
                entity.HasData(
                    new Product { Id = 1, Name = "Product 1", Description = "Description for Product 1", Price = 10.00m, Status = PublicationStatus.Published },
                    new Product { Id = 2, Name = "Product 2", Description = "Description for Product 2", Price = 20.00m, Status = PublicationStatus.Hidden },
                    new Product { Id = 3, Name = "Product 3", Description = "Description for Product 3", Price = 30.00m, Status = PublicationStatus.Published },
                    new Product { Id = 4, Name = "Product 4", Description = "Description for Product 4", Price = 20.00m, Status = PublicationStatus.Hidden },
                    new Product { Id = 5, Name = "Product 5", Description = "Description for Product 5", Price = 30.00m, Status = PublicationStatus.Published }
                );
            });
        }
    }
}
