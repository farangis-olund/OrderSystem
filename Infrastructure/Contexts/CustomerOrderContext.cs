using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class CustomerOrderContext(DbContextOptions<CustomerOrderContext> options) : DbContext(options)
{
    public virtual DbSet<CustomerEntity> Customers { get; set; }
    public virtual DbSet<CustomerOrderEntity> CustomerOrders { get; set; }
    public virtual DbSet<OrderDetailEntity> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerEntity>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<OrderDetailEntity>()
            .HasIndex(x => new { x.CustomerOrderId, x.ProductVariantId })
            .IsUnique();

        // Ignore entities from the database-first context
        modelBuilder.Ignore<BrandEntity>();
        modelBuilder.Ignore<CategoryEntity>();
        modelBuilder.Ignore<ColorEntity>();
        modelBuilder.Ignore<CurrencyEntity>();
        modelBuilder.Ignore<ImageEntity>();
        modelBuilder.Ignore<ProductEntity>();
        modelBuilder.Ignore<ProductImageEntity>();
        modelBuilder.Ignore<ProductPriceEntity>();
        modelBuilder.Ignore<ProductVariantEntity>();
        modelBuilder.Ignore<SizeEntity>();



    }
}
