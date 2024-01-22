
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<ProductDataContext, CategoryEntity>
{
    public CategoryRepository(ProductDataContext context, ILogger<BaseRepository<ProductDataContext, CategoryEntity>> logger) : base(context, logger)
    {
    }

    public override Task<CategoryEntity> AddAsync(CategoryEntity entity)
    {
        return base.AddAsync(entity);
    }

    public override Task<bool> Exist(Expression<Func<CategoryEntity, bool>> predicate)
    {
        return base.Exist(predicate);
    }

    public override Task<IEnumerable<CategoryEntity>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<CategoryEntity> GetOneAsync(Expression<Func<CategoryEntity, bool>> predicate)
    {
        return base.GetOneAsync(predicate);
    }

    public override Task<CategoryEntity?> GetOneAsync(Expression<Func<CategoryEntity, bool>> predicate, Func<Task<CategoryEntity>> createIfNotFound)
    {
        return base.GetOneAsync(predicate, createIfNotFound);
    }

    public override Task<bool> RemoveAsync(Expression<Func<CategoryEntity, bool>> predicate)
    {
        return base.RemoveAsync(predicate);
    }

    public override Task<CategoryEntity> UpdateAsync(CategoryEntity entity)
    {
        return base.UpdateAsync(entity);
    }
}
