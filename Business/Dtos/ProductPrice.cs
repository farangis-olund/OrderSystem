namespace Business.Dtos;

public class ProductPrice
{
    public int Id { get; set; }
    public int ProductVariantId { get; set; }
    public string ArticleNumber { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public decimal DicountPercentage { get; set; }
    public string Code { get; set; } = null!;
    public string CurrencyName { get; set; }= null!;
}
