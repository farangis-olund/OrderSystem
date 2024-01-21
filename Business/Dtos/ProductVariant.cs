
namespace Business.Dtos;

public class ProductVariant
{
    public int Id { get; set; }
    public string ArticleNumber { get; set; } = null!;
    public int Quantity { get; set; }
    public int SizeId { get; set; }
    public int ColorId { get; set; }
    public ProductPrice Price { get; set; } = null!;
    public ProductSize Size { get; set; } = null!;
    public string ColorName { get; set; } = null!;
    public List<ProductImage> ImageUrls { get; set; } = [];

}
