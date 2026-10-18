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
                entity.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
                entity.HasData(
                    new Product { Id = 1, Name = "Nike Air Max", Price = 170.00m, Status = PublicationStatus.Published },
                    new Product { Id = 2, Name = "Adidas Ultraboost", Description = "Version 3.0", Price = 260.00m, Status = PublicationStatus.Published },
                    new Product { Id = 3, Name = "Nike Air Super Max", Description = "Model 2025", Price = 303.00m, Status = PublicationStatus.Published },
                    new Product { Id = 4, Name = "Converse Chuck Taylor All Star", Price = 230.00m, Status = PublicationStatus.Published },
                    new Product { Id = 5, Name = "Vans Old Skool", Description = "New colors", Price = 303.00m, Status = PublicationStatus.Published },
                    new Product { Id = 6, Name = "Nike Air Jordan", Description = "Unisex", Price = 340.00m, Status = PublicationStatus.Hidden }
                );
            });
        }
    }
}
