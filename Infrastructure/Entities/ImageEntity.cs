using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index("ImageUrl", Name = "UQ__ImageEnt__372CE60DA0480FAA", IsUnique = true)]
public partial class ImageEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string ImageUrl { get; set; } = null!;

    [InverseProperty("Image")]
    public virtual ICollection<ProductImageEntity> ProductImageEntities { get; set; } = new List<ProductImageEntity>();
}
