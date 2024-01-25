﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index("SizeType", "SizeValue", "AgeGroup", Name = "UC_Size", IsUnique = true)]
public partial class SizeEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? SizeType { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string? SizeValue { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string? AgeGroup { get; set; }

    [InverseProperty("Size")]
    public virtual ICollection<ProductVariantEntity> ProductVariantEntities { get; set; } = new List<ProductVariantEntity>();

    public static implicit operator SizeEntity(ProductSize productSize)
    {
        return new SizeEntity
        {
            SizeType = productSize.SizeType,
            SizeValue = productSize.SizeValue,
            AgeGroup = productSize.AgeGroup
        };
    }
}
