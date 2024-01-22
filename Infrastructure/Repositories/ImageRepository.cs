
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ImageRepository : BaseRepository<ProductDataContext, ImageEntity>
{
    public ImageRepository(ProductDataContext context, ILogger<ImageRepository> logger)
        : base(context, logger)
    {

    }

    public override Task<ImageEntity> AddAsync(ImageEntity entity)
    {
        return base.AddAsync(entity);
    }

    public override Task<bool> Exist(Expression<Func<ImageEntity, bool>> predicate)
    {
        return base.Exist(predicate);
    }

    public override Task<IEnumerable<ImageEntity>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<ImageEntity> GetOneAsync(Expression<Func<ImageEntity, bool>> predicate)
    {
        return base.GetOneAsync(predicate);
    }

    public override Task<ImageEntity?> GetOneAsync(Expression<Func<ImageEntity, bool>> predicate, Func<Task<ImageEntity>> createIfNotFound)
    {
        return base.GetOneAsync(predicate, createIfNotFound);
    }

    public override Task<bool> RemoveAsync(Expression<Func<ImageEntity, bool>> predicate)
    {
        return base.RemoveAsync(predicate);
    }

    public override Task<ImageEntity> UpdateAsync(ImageEntity entity)
    {
        return base.UpdateAsync(entity);
    }
}
