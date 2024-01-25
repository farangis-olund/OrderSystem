using Infrastructure.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public static implicit operator CategoryEntity(Category category)
    {
        return new CategoryEntity
        {
            CategoryName = category.CategoryName,
            ParentCategoryId = category.ParentCategoryId

        };
    }

    public static implicit operator CategoryEntity(ProductDetail productDetail)
    {
        return new CategoryEntity
        {
            CategoryName = productDetail.CategoryName,
         
        };
    }
}
