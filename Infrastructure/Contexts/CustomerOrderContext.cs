using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class CustomerOrderContext : DbContext
    {
        public CustomerOrderContext(DbContextOptions<CustomerOrderContext> options) : base(options)
        {
        }

        public virtual DbSet<CustomerEntity> Customers { get; set; }
        public virtual DbSet<CustomerOrderEntity> CustomerOrders { get; set; }
        public virtual DbSet<OrderDetailEntity> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEntity>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<CustomerOrderEntity>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerOrders)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerOrder_Customer");
            });

            modelBuilder.Entity<OrderDetailEntity>()
                .HasIndex(x => new { x.CustomerOrderId, x.ProductVariantId })
                .IsUnique();

            modelBuilder.Entity<OrderDetailEntity>(entity =>
            {
                entity.HasOne(c => c.CustomerOrder)
                    .WithMany(z => z.OrderDetails)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_CustomerOrder");
            });

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
}
