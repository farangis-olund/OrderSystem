using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Dtos;

namespace Infrastructure.Entities;

[Index("ProductVariantId", "ImageId", Name = "UC_ProductImage", IsUnique = true)]
public partial class ProductImageEntity
{
    [Key]
    public int Id { get; set; }

    public int ProductVariantId { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string ArticleNumber { get; set; } = null!;

    public int ImageId { get; set; }

    [ForeignKey("ImageId")]
    [InverseProperty("ProductImageEntities")]
    public virtual ImageEntity Image { get; set; } = null!;

    [ForeignKey("ProductVariantId")]
    [InverseProperty("ProductImageEntities")]
    public virtual ProductVariantEntity ProductVariant { get; set; } = null!;

    public static implicit operator ProductImageEntity(ProductImage productImage)
    {
        return new ProductImageEntity
        {
           ProductVariantId = productImage.ProductVariantId,
           ArticleNumber = productImage.ArticleNumber,
           ImageId = productImage.ImageId

        };
    }
}
