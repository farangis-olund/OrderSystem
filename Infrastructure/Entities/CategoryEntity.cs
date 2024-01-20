using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

public partial class CategoryEntity
{
    [Key]
    public int Id { get; set; }

    public int? ParentCategoryId { get; set; }

    [StringLength(50)]
    public string CategoryName { get; set; } = null!;

    [InverseProperty("ParentCategory")]
    public virtual ICollection<CategoryEntity> InverseParentCategory { get; set; } = new List<CategoryEntity>();

    [ForeignKey("ParentCategoryId")]
    [InverseProperty("InverseParentCategory")]
    public virtual CategoryEntity? ParentCategory { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<ProductEntity> ProductEntities { get; set; } = new List<ProductEntity>();
}
