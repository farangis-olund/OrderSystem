using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index("ArticleNumber", "SizeId", "ColorId", Name = "UC_ProductVariant", IsUnique = true)]
public partial class ProductVariantEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string ArticleNumber { get; set; } = null!;

    public int Quantity { get; set; }

    public int SizeId { get; set; }

    public int ColorId { get; set; }

    [ForeignKey("ArticleNumber")]
    [InverseProperty("ProductVariantEntities")]
    public virtual ProductEntity ArticleNumberNavigation { get; set; } = null!;

    [ForeignKey("ColorId")]
    [InverseProperty("ProductVariantEntities")]
    public virtual ColorEntity Color { get; set; } = null!;

    [InverseProperty("ProductVariant")]
    public virtual ICollection<ProductImageEntity> ProductImageEntities { get; set; } = new List<ProductImageEntity>();

    [InverseProperty("ProductVariant")]
    public virtual ICollection<ProductPriceEntity> ProductPriceEntities { get; set; } = new List<ProductPriceEntity>();

    [ForeignKey("SizeId")]
    [InverseProperty("ProductVariantEntities")]
    public virtual SizeEntity Size { get; set; } = null!;

    public static implicit operator ProductVariantEntity(ProductDetail productVariant)
    {
        return new ProductVariantEntity
        {
            ArticleNumber = productVariant.ArticleNumber,
            Quantity = productVariant.Quantity,
            SizeId = productVariant.SizeId,
            ColorId = productVariant.ColorId,
            Size = productVariant.Size,
            Color = productVariant.Color,
            ArticleNumberNavigation = productVariant.Product
   
        };

      
    }



}

