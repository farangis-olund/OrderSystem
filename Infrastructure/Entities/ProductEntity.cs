﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Dtos;

namespace Infrastructure.Entities;

public partial class ProductEntity
{
    [Key]
    [StringLength(30)]
    [Unicode(false)]
    public string ArticleNumber { get; set; } = null!;

    [StringLength(200)]
    public string ProductName { get; set; } = null!;

    public string? Material { get; set; }

    public string? ProductInfo { get; set; }

    public int CategoryId { get; set; }

    public int BrandId { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("ProductEntities")]
    public virtual BrandEntity Brand { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("ProductEntities")]
    public virtual CategoryEntity Category { get; set; } = null!;

    [InverseProperty("ArticleNumberNavigation")]
    public virtual ICollection<ProductVariantEntity> ProductVariantEntities { get; set; } = new List<ProductVariantEntity>();

    public static implicit operator ProductEntity(Product product)
    {
        return new ProductEntity
        {
            ArticleNumber = product.ArticleNumber,
            ProductName = product.ProductName,
            Material = product.Material,
            ProductInfo = product.ProductInfo,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId

        };
    }

    public static implicit operator ProductEntity(ProductDetail product)
    {
        return new ProductEntity
        {
            ArticleNumber = product.ArticleNumber,
            ProductName = product.ProductName,
            Material = product.Material,
            ProductInfo = product.ProductInfo,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId

        };
    }
}
