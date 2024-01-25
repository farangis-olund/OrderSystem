
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class CurrencyRepository : BaseRepository<ProductDataContext, CurrencyEntity>
    {
        public CurrencyRepository(ProductDataContext context, ILogger<CurrencyRepository> logger)
            : base(context, logger)
        {

        }

        public override Task<CurrencyEntity> AddAsync(CurrencyEntity entity)
        {
            return base.AddAsync(entity);
        }

        public override Task<bool> Exist(Expression<Func<CurrencyEntity, bool>> predicate)
        {
            return base.Exist(predicate);
        }

        public override Task<IEnumerable<CurrencyEntity>> GetAllAsync()
        {
            return base.GetAllAsync();
        }

        public override Task<CurrencyEntity> GetOneAsync(Expression<Func<CurrencyEntity, bool>> predicate)
        {
            return base.GetOneAsync(predicate);
        }

        public override Task<CurrencyEntity> GetOneAsync(Expression<Func<CurrencyEntity, bool>> predicate, Func<Task<CurrencyEntity>> createIfNotFound)
        {
            return base.GetOneAsync(predicate, createIfNotFound);
        }

        public override Task<bool> RemoveAsync(Expression<Func<CurrencyEntity, bool>> predicate)
        {
            return base.RemoveAsync(predicate);
        }

        public override Task<bool> RemoveAsync(CurrencyEntity entity)
        {
            return base.RemoveAsync(entity);
        }

        public override Task<CurrencyEntity> UpdateAsync(CurrencyEntity entity, Func<CurrencyEntity, object> keySelector)
        {
            return base.UpdateAsync(entity, keySelector);
        }
    }
}
