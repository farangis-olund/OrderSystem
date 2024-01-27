using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class ProductVariant
{
   
    public string ArticleNumber { get; set; } = null!;
    public int Quantity { get; set; }
    public int SizeId { get; set; }
    public int ColorId { get; set; }
    public Color Color { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public ProductPrice Price { get; set; } = null!;
    public ProductSize Size { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public Brand Brand { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public List<ProductImage> ProductImages { get; set; } = null!;
    public Currency Currency { get; set; } = null!;


    public static implicit operator ProductVariant(ProductVariantEntity entity)
    {
        return new ProductVariant
        {
            ArticleNumber = entity.ArticleNumber,
            Quantity = entity.Quantity,
            SizeId = entity.SizeId,
            ColorId = entity.ColorId,
            ProductName = entity.ArticleNumberNavigation.ProductName,
            ImageUrl = entity.ProductImageEntities.Select(pi => pi.Image).Select(img => img.ImageUrl).FirstOrDefault()!,
            Size = entity.Size,
            Color = entity.Color,
            Product = entity.ArticleNumberNavigation,
            Brand = entity.ArticleNumberNavigation?.Brand!,
            Category = entity.ArticleNumberNavigation?.Category!,
            ProductImages = entity.ProductImageEntities?.Select(pi => (ProductImage)pi).ToList() ?? new List<ProductImage>(),
            Price = entity.ProductPriceEntities?.FirstOrDefault() ?? new ProductPrice(),
            Currency = entity.ProductPriceEntities?.FirstOrDefault()?.CurrencyCodeNavigation ?? new Currency()
        };
    }

}
