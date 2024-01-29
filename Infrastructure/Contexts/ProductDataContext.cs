using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class ProductDataContext : DbContext
{
    public ProductDataContext()
    {
    }

    public ProductDataContext(DbContextOptions<ProductDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BrandEntity> BrandEntities { get; set; }

    public virtual DbSet<CategoryEntity> CategoryEntities { get; set; }

    public virtual DbSet<ColorEntity> ColorEntities { get; set; }

    public virtual DbSet<CurrencyEntity> CurrencyEntities { get; set; }

    public virtual DbSet<ImageEntity> ImageEntities { get; set; }

    public virtual DbSet<ProductEntity> ProductEntities { get; set; }

    public virtual DbSet<ProductImageEntity> ProductImageEntities { get; set; }

    public virtual DbSet<ProductPriceEntity> ProductPriceEntities { get; set; }

    public virtual DbSet<ProductVariantEntity> ProductVariantEntities { get; set; }

    public virtual DbSet<SizeEntity> SizeEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrandEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BrandEnt__3214EC07E0FA49B3");
        });

        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC074E26CE24");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory).HasConstraintName("FK__CategoryE__Paren__690797E6");
        });

        modelBuilder.Entity<ColorEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ColorEnt__3214EC0713FF3C36");
        });

        modelBuilder.Entity<CurrencyEntity>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__Currency__A25C5AA611A43D25");

            entity.Property(e => e.Code).IsFixedLength();
        });

        modelBuilder.Entity<ImageEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ImageEnt__3214EC077E7764F8");
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__ProductE__3C991143B258182C");

            entity.HasOne(d => d.Brand).WithMany(p => p.ProductEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductEn__Brand__7849DB76");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductEn__Categ__7755B73D");
        });

        modelBuilder.Entity<ProductImageEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC07AF2DAD5C");

            entity.HasOne(d => d.Image).WithMany(p => p.ProductImageEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductIm__Image__078C1F06");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.ProductImageEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductIm__Produ__0697FACD");
        });

        modelBuilder.Entity<ProductPriceEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductP__3214EC07358C90BE");

            entity.Property(e => e.CurrencyCode).IsFixedLength();

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.ProductPriceEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductPr__Curre__02C769E9");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.ProductPriceEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductPr__Produ__01D345B0");
        });

        modelBuilder.Entity<ProductVariantEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductV__3214EC0756452609");

            entity.HasOne(d => d.ArticleNumberNavigation).WithMany(p => p.ProductVariantEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__Artic__7C1A6C5A");

            entity.HasOne(d => d.Color).WithMany(p => p.ProductVariantEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__Color__7E02B4CC");

            entity.HasOne(d => d.Size).WithMany(p => p.ProductVariantEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__SizeI__7D0E9093");
        });

        modelBuilder.Entity<SizeEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SizeEnti__3214EC07038070CD");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
