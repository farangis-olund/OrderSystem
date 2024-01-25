
using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class Product 
{
    public string ArticleNumber { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string Material { get; set; } = null!;
    public string ProductInfo { get; set; } = null!;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public int BrandId { get; set; }
    public string BrandName { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public Brand Brand { get; set; } = null!;

    public static implicit operator Product(ProductEntity entity)
    {
        return new Product
        {
            ArticleNumber = entity.ArticleNumber,
            ProductName = entity.ProductName,
            Material = entity.Material!,
            ProductInfo = entity.ProductInfo!,
            CategoryId = entity.CategoryId,
            BrandId = entity.BrandId
           
        };
    }

    public static implicit operator Product(ProductDetail productDetail)
    {
        return new Product
        {
            ArticleNumber = productDetail.ArticleNumber,
            ProductName = productDetail.ProductName,
            Material = productDetail.Material!,
            ProductInfo = productDetail.ProductInfo!,
            CategoryName = productDetail.CategoryName,
            BrandName = productDetail.BrandName,
            BrandId = productDetail.BrandId,
            CategoryId = productDetail.CategoryId


        };
    }
}
