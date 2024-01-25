using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BrandRepository : BaseRepository<ProductDataContext, BrandEntity>
    {
        public BrandRepository(ProductDataContext context, ILogger<BrandRepository> logger)
            : base(context, logger)
        {
            
        }

        public override Task<BrandEntity> AddAsync(BrandEntity entity)
        {
            return base.AddAsync(entity);
        }

        public override Task<bool> Exist(Expression<Func<BrandEntity, bool>> predicate)
        {
            return base.Exist(predicate);
        }

        public override Task<IEnumerable<BrandEntity>> GetAllAsync()
        {
            return base.GetAllAsync();
        }

        public override Task<BrandEntity> GetOneAsync(Expression<Func<BrandEntity, bool>> predicate)
        {
            return base.GetOneAsync(predicate);
        }

        public override Task<BrandEntity> GetOneAsync(Expression<Func<BrandEntity, bool>> predicate, Func<Task<BrandEntity>> createIfNotFound)
        {
            return base.GetOneAsync(predicate, createIfNotFound);
        }

        public override Task<bool> RemoveAsync(Expression<Func<BrandEntity, bool>> predicate)
        {
            return base.RemoveAsync(predicate);
        }

        public override Task<bool> RemoveAsync(BrandEntity entity)
        {
            return base.RemoveAsync(entity);
        }

        public override Task<BrandEntity> UpdateAsync(BrandEntity entity, Func<BrandEntity, object> keySelector)
        {
            return base.UpdateAsync(entity, keySelector);
        }
    }
}
