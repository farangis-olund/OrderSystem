
using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class Category 
{
    public int Id { get; set; }
    public int? ParentCategoryId { get; set; }
    public string CategoryName { get; set; } = null!;

    public static implicit operator Category(CategoryEntity entity)
    {
        return new Category
        {
            ParentCategoryId = entity.ParentCategoryId,
            CategoryName = entity.CategoryName

        };
    }

}
