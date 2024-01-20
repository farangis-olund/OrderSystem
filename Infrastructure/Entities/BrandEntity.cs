using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index("BrandName", Name = "UQ__BrandEnt__2206CE9B44471B25", IsUnique = true)]
public partial class BrandEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string BrandName { get; set; } = null!;

    [InverseProperty("Brand")]
    public virtual ICollection<ProductEntity> ProductEntities { get; set; } = new List<ProductEntity>();
}
