﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index("ColorName", Name = "UQ__ColorEnt__C71A5A7B3F09DBC7", IsUnique = true)]
public partial class ColorEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ColorName { get; set; } = null!;

    [InverseProperty("Color")]
    public virtual ICollection<ProductVariantEntity> ProductVariantEntities { get; set; } = new List<ProductVariantEntity>();
}