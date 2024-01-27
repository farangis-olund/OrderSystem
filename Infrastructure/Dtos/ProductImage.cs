
using Infrastructure.Entities;

namespace Infrastructure.Dtos
{
    public class ProductImage
    {
        public int ProductVariantId { get; set; }
        public string ArticleNumber { get; set; } = null!;
        public int ImageId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public Image Image { get; set; } = null!;
        public ProductVariant ProductVariant { get; set; } = null!;

        public static implicit operator ProductImage(ProductImageEntity entity)
        {
            return new ProductImage
            {
                ProductVariantId = entity.ProductVariantId,
                ArticleNumber = entity.ArticleNumber,
                ImageId = entity.ImageId,
                Image = entity.Image

            };
        }
    }
}
