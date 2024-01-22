
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ProductImageRepository : BaseRepository<ProductDataContext, ProductImageEntity>
{
    public ProductImageRepository(ProductDataContext context, ILogger<ProductImageRepository> logger)
        : base(context, logger)
    {

    }

    public override Task<ProductImageEntity> AddAsync(ProductImageEntity entity)
    {
        return base.AddAsync(entity);
    }

    public override Task<bool> Exist(Expression<Func<ProductImageEntity, bool>> predicate)
    {
        return base.Exist(predicate);
    }

    public override Task<IEnumerable<ProductImageEntity>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<ProductImageEntity?> GetOneAsync(Expression<Func<ProductImageEntity, bool>> predicate, Func<Task<ProductImageEntity>> createIfNotFound)
    {
        return base.GetOneAsync(predicate, createIfNotFound);
    }

    public override Task<ProductImageEntity> GetOneAsync(Expression<Func<ProductImageEntity, bool>> predicate)
    {
        return base.GetOneAsync(predicate);
    }

    public override Task<bool> RemoveAsync(Expression<Func<ProductImageEntity, bool>> predicate)
    {
        return base.RemoveAsync(predicate);
    }

    public override Task<ProductImageEntity> UpdateAsync(ProductImageEntity entity)
    {
        return base.UpdateAsync(entity);
    }
}
