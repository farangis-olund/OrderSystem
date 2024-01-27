using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class ProductPrice
{
    public int ProductVariantId { get; set; }
    public string ArticleNumber { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public decimal DicountPercentage { get; set; }
    public string Code { get; set; } = null!;
    public string CurrencyName { get; set; }= null!;
    public Currency Currency { get; set; } = null!;
    public ProductVariant ProductVariant { get; set; } = null!;

    public static implicit operator ProductPrice(ProductPriceEntity entity)
    {
        return new ProductPrice
        {
            ProductVariantId = entity.ProductVariantId,
            ArticleNumber = entity.ArticleNumber,
            Price = entity.Price,
            DicountPercentage = entity.DicountPercentage ?? 0,
            DiscountPrice = entity.DiscountPrice ?? 0,
            Code = entity.CurrencyCode,
            Currency = entity.CurrencyCodeNavigation,
            //ProductVariant = entity.ProductVariant
           
        };
    }
}
