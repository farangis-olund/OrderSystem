
using Infrastructure.Entities;
using System.Drawing;

namespace Infrastructure.Dtos
{
    public class ProductDetail
    {
        public string ArticleNumber { get; set; } = null!;
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } = null!;
        public string Material { get; set; } = null!;
        public string ProductInfo { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public int BrandId { get; set; } 
        public string ImageUrl { get; set; } = null!;
        public int ColorId { get; set; }
        public string ColorName { get; set; } = null!;
        public string SizeValue { get; set; } = null!;
        public int SizeId { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal DicountPercentage { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string CurrencyName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public int CategoryId { get; set; }
        public Color Color { get; set; } = null!;
        public ProductPrice ProductPrice { get; set; } = null!;
        public ProductSize Size { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public Brand Brand { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public List<ProductImage> ProductImages { get; set; } = null!;
        public Currency Currency { get; set; } = null!;
        public  bool Selected { get; set; }
        public int OrderQuantity { get; set; }

        public static implicit operator ProductDetail(ProductVariantEntity entity)
        {
            return new ProductDetail
            {
                ArticleNumber = entity.ArticleNumber,
                Quantity = entity.Quantity,
                ProductName = entity.ArticleNumberNavigation.ProductName,
                ImageUrl = entity.ProductImageEntities.Select(pi => pi.Image).Select(img => img.ImageUrl).FirstOrDefault()!,
                Price = entity.ProductPriceEntities.Select(pi => pi.Price).FirstOrDefault()!,
                ProductInfo = entity.ArticleNumberNavigation.ProductInfo!,
                Material = entity.ArticleNumberNavigation.Material!,
                BrandName = entity.ArticleNumberNavigation.Brand.BrandName!,
                CategoryName = entity.ArticleNumberNavigation.Category.CategoryName,
                ColorId = entity.ColorId,
                ColorName = entity.Color.ColorName,
                SizeValue = entity.Size.SizeValue!,
                CurrencyCode =entity.ProductPriceEntities.Select(c => c.CurrencyCode).FirstOrDefault()!,
                Size = entity.Size,
                Color = entity.Color,
                Product = entity.ArticleNumberNavigation,
                Brand = entity.ArticleNumberNavigation?.Brand!,
                Category = entity.ArticleNumberNavigation?.Category!,
                ProductImages = entity.ProductImageEntities?.Select(pi => (ProductImage)pi).ToList() ?? new List<ProductImage>(),
                ProductPrice = entity.ProductPriceEntities?.FirstOrDefault() ?? new ProductPrice(),
                Currency = entity.ProductPriceEntities?.FirstOrDefault()?.CurrencyCodeNavigation ?? new Currency()
            };
        }

    }
}
