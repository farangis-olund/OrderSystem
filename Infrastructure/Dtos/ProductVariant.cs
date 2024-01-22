﻿
using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class ProductVariant
{
    public string ArticleNumber { get; set; } = null!;
    public int Quantity { get; set; }
    public int SizeId { get; set; }
    public int ColorId { get; set; }
    public string ColorName { get; set; } = null!;
    public ProductPrice Price { get; set; } = null!;
    public ProductSize Size { get; set; } = null!;
    public Product Product { get; set; } = null!;

    public static implicit operator ProductVariant(ProductVariantEntity entity)
    {
        return new ProductVariant
        {
            Quantity = entity.Quantity,
            SizeId = entity.SizeId,
            ColorId = entity.ColorId,
            Size = entity.Size
            
        };
    }
}