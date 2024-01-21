using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index("ProductVariantId", "Price", Name = "UC_ProductPrice", IsUnique = true)]
public partial class ProductPriceEntity
{
    [Key]
    public int Id { get; set; }

    public int ProductVariantId { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string ArticleNumber { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [Column(TypeName = "money")]
    public decimal? DiscountPrice { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? DicountPercentage { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string CurrencyCode { get; set; } = null!;

    [ForeignKey("CurrencyCode")]
    [InverseProperty("ProductPriceEntities")]
    public virtual CurrencyEntity CurrencyCodeNavigation { get; set; } = null!;

    [ForeignKey("ProductVariantId")]
    [InverseProperty("ProductPriceEntities")]
    public virtual ProductVariantEntity ProductVariant { get; set; } = null!;
}
