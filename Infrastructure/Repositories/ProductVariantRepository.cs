using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ProductVariantRepository : BaseRepository<ProductDataContext, ProductVariantEntity>
{
    public ProductVariantRepository(ProductDataContext context, ILogger<ProductVariantRepository> logger)
        : base(context, logger)
    {

    }

    public override Task<ProductVariantEntity> AddAsync(ProductVariantEntity entity)
    {
        return base.AddAsync(entity);
    }

    public override Task<bool> Exist(Expression<Func<ProductVariantEntity, bool>> predicate)
    {
        return base.Exist(predicate);
    }

    public override Task<IEnumerable<ProductVariantEntity>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<ProductVariantEntity> GetOneAsync(Expression<Func<ProductVariantEntity, bool>> predicate, Func<Task<ProductVariantEntity>> createIfNotFound)
    {
        return base.GetOneAsync(predicate, createIfNotFound);
    }

    public override Task<ProductVariantEntity> GetOneAsync(Expression<Func<ProductVariantEntity, bool>> predicate)
    {
        return base.GetOneAsync(predicate);
    }

    public override Task<bool> RemoveAsync(Expression<Func<ProductVariantEntity, bool>> predicate)
    {
        return base.RemoveAsync(predicate);
    }

    public override Task<bool> RemoveAsync(ProductVariantEntity entity)
    {
        return base.RemoveAsync(entity);
    }

    public override Task<ProductVariantEntity> UpdateAsync(ProductVariantEntity entity, Func<ProductVariantEntity, object> keySelector)
    {
        return base.UpdateAsync(entity, keySelector);
    }
}
