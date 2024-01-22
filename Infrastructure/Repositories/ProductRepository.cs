
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ProductRepository : BaseRepository<ProductDataContext, ProductEntity>
{
    public ProductRepository(ProductDataContext context, ILogger<BaseRepository<ProductDataContext, ProductEntity>> logger) 
        : base(context, logger)
    {
    }

    public override Task<ProductEntity> AddAsync(ProductEntity entity)
    {
        return base.AddAsync(entity);
    }

    public override Task<bool> Exist(Expression<Func<ProductEntity, bool>> predicate)
    {
        return base.Exist(predicate);
    }

    public override Task<IEnumerable<ProductEntity>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<ProductEntity?> GetOneAsync(Expression<Func<ProductEntity, bool>> predicate, Func<Task<ProductEntity>> createIfNotFound)
    {
        return base.GetOneAsync(predicate, createIfNotFound);
    }

    public override Task<ProductEntity> GetOneAsync(Expression<Func<ProductEntity, bool>> predicate)
    {
        return base.GetOneAsync(predicate);
    }

    public override Task<bool> RemoveAsync(Expression<Func<ProductEntity, bool>> predicate)
    {
        return base.RemoveAsync(predicate);
    }

    public override Task<ProductEntity> UpdateAsync(ProductEntity entity)
    {
        return base.UpdateAsync(entity);
    }
}
