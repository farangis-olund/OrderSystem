
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ProductPriceRepository : BaseRepository<ProductDataContext, ProductPriceEntity>
{
    public ProductPriceRepository(ProductDataContext context, ILogger<ProductPriceRepository> logger)
        : base(context, logger)
    {

    }

    public override Task<ProductPriceEntity> AddAsync(ProductPriceEntity entity)
    {
        return base.AddAsync(entity);
    }

    public override Task<bool> Exist(Expression<Func<ProductPriceEntity, bool>> predicate)
    {
        return base.Exist(predicate);
    }

    public override Task<IEnumerable<ProductPriceEntity>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<ProductPriceEntity?> GetOneAsync(Expression<Func<ProductPriceEntity, bool>> predicate, Func<Task<ProductPriceEntity>> createIfNotFound)
    {
        return base.GetOneAsync(predicate, createIfNotFound);
    }

    public override Task<ProductPriceEntity> GetOneAsync(Expression<Func<ProductPriceEntity, bool>> predicate)
    {
        return base.GetOneAsync(predicate);
    }

    public override Task<bool> RemoveAsync(Expression<Func<ProductPriceEntity, bool>> predicate)
    {
        return base.RemoveAsync(predicate);
    }

    public override Task<bool> UpdateAsync(ProductPriceEntity entity)
    {
        return base.UpdateAsync(entity);
    }
}
