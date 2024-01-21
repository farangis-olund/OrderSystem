
namespace Business.Dtos
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string ArticleNumber { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
