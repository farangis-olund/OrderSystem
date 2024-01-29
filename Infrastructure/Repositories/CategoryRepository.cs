
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<ProductDataContext, CategoryEntity>
{
    public CategoryRepository(ProductDataContext context) : base(context)
    {
    }

   
}
