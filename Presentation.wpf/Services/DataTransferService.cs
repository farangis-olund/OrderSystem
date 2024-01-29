using Infrastructure.Dtos;
using Infrastructure.Entities;

namespace Presentation.wpf.Services;

public class DataTransferService
{
    public ProductDetail SelectedProductItem { get; set; } = null!;
    public Customer SelectedCustomerItem { get; set; } = null!;
    public OrderDetail SelectedOrderItem { get; set; } = null!;
    public IEnumerable<ProductDetail> ConvertToProductDetails(IEnumerable<ProductVariantEntity> entities)
    {
        return entities.Select(ConvertToProductDetail);
    }

    public ProductDetail ConvertToProductDetail(ProductVariantEntity entity)
    {
        return new ProductDetail
        {
            ArticleNumber = entity.ArticleNumber,
            Quantity = entity.Quantity,
            ProductName = entity.ArticleNumberNavigation.ProductName,
            ImageUrl = entity.ProductImageEntities.Select(pi => pi.Image?.ImageUrl).FirstOrDefault()!,
            Price = entity.ProductPriceEntities.Select(pi => pi.Price).FirstOrDefault()!,
            ProductInfo = entity.ArticleNumberNavigation.ProductInfo!,
            Material = entity.ArticleNumberNavigation.Material!,
            BrandName = entity.ArticleNumberNavigation.Brand.BrandName!,
            CategoryName = entity.ArticleNumberNavigation.Category.CategoryName,
            ColorName = entity.Color.ColorName,
            SizeValue = entity.Size.SizeValue!,
            CurrencyCode = entity.ProductPriceEntities.Select(c => c.CurrencyCode).FirstOrDefault()!,
            Size = entity.Size,
            Color = entity.Color,
            Product = entity.ArticleNumberNavigation,
            Brand = entity.ArticleNumberNavigation?.Brand!,
            Category = entity.ArticleNumberNavigation?.Category!,
            ProductImages = entity.ProductImageEntities.Select(pi => (ProductImage)pi).ToList() ?? new List<ProductImage>(),
            ProductPrice = entity.ProductPriceEntities.FirstOrDefault() ?? new ProductPrice(),
            Currency = entity.ProductPriceEntities.FirstOrDefault()?.CurrencyCodeNavigation ?? new Currency()
        };
    }


    public IEnumerable<OrderDetail> ConvertToOrderDetails(IEnumerable<OrderDetailEntity> entities)
    {
        return entities.Select(ConvertToOrderDetail);
    }

    public OrderDetail ConvertToOrderDetail(OrderDetailEntity entity)
    {
        return new OrderDetail
        {
           CustomerOrderId = entity.CustomerOrderId,
           ProductVariantId = entity.ProductVariantId,
           Quantity = entity.Quantity,
           CustomerOrder = entity.CustomerOrder,
           Customer = entity.CustomerOrder?.Customer!

        };
    }

}