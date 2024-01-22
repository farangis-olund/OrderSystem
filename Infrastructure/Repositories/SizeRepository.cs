
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class SizeRepository : BaseRepository<ProductDataContext, SizeEntity>
{
    public SizeRepository(ProductDataContext context, ILogger<SizeRepository> logger)
        : base(context, logger)
    {

    }

    public override Task<SizeEntity> AddAsync(SizeEntity entity)
    {
        return base.AddAsync(entity);
    }

    public override Task<bool> Exist(Expression<Func<SizeEntity, bool>> predicate)
    {
        return base.Exist(predicate);
    }

    public override Task<IEnumerable<SizeEntity>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<SizeEntity?> GetOneAsync(Expression<Func<SizeEntity, bool>> predicate, Func<Task<SizeEntity>> createIfNotFound)
    {
        return base.GetOneAsync(predicate, createIfNotFound);
    }

    public override Task<SizeEntity> GetOneAsync(Expression<Func<SizeEntity, bool>> predicate)
    {
        return base.GetOneAsync(predicate);
    }

    public override Task<bool> RemoveAsync(Expression<Func<SizeEntity, bool>> predicate)
    {
        return base.RemoveAsync(predicate);
    }

    public override Task<SizeEntity> UpdateAsync(SizeEntity entity)
    {
        return base.UpdateAsync(entity);
    }
}
