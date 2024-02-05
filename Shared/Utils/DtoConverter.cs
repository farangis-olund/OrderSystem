
using Infrastructure.Dtos;
using Infrastructure.Entities;

namespace Shared.Utils
{
    public class DtoConverter
    {
        public IEnumerable<ProductDetail> ConvertToProductDetails(IEnumerable<ProductVariantEntity> entities)
        {
            return entities.Select(ConvertToProductDetail);
        }

        public ProductDetail ConvertToProductDetail(ProductVariantEntity entity)
        {
            return new ProductDetail
            {
                ArticleNumber = entity.ArticleNumber,
                ProductVariantId = entity.Id,
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
                ProductImages = entity.ProductImageEntities.Select(pi => (ProductImage)pi).ToList() ?? [],
                ProductPrice = entity.ProductPriceEntities.FirstOrDefault() ?? new ProductPrice(),
                Currency = entity.ProductPriceEntities.FirstOrDefault()?.CurrencyCodeNavigation ?? new Currency()
            };
        }


        public static IEnumerable<CustomerOrder> ConvertToOrderDetails(IEnumerable<CustomerOrderEntity> entities)
        {
            return entities.Select(ConvertToCustomerOrder);
        }

        public static CustomerOrder ConvertToCustomerOrder(CustomerOrderEntity entity)
        {
            return new CustomerOrder
            {
                CustomerOrderId = entity.Id,
                CustomerId = entity.CustomerId,
                Date = entity.Date,
                TotalAmount = entity.TotalAmount,
                Customer = entity.Customer

            };
        }
    }
}
