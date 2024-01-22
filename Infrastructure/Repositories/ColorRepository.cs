
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class ColorRepository : BaseRepository<ProductDataContext, ColorEntity>
    {
        public ColorRepository(ProductDataContext context, ILogger<ColorRepository> logger)
            : base(context, logger)
        {

        }

        public override Task<ColorEntity> AddAsync(ColorEntity entity)
        {
            return base.AddAsync(entity);
        }

        public override Task<bool> Exist(Expression<Func<ColorEntity, bool>> predicate)
        {
            return base.Exist(predicate);
        }

        public override Task<IEnumerable<ColorEntity>> GetAllAsync()
        {
            return base.GetAllAsync();
        }

        public override Task<ColorEntity> GetOneAsync(Expression<Func<ColorEntity, bool>> predicate)
        {
            return base.GetOneAsync(predicate);
        }

        public override Task<ColorEntity?> GetOneAsync(Expression<Func<ColorEntity, bool>> predicate, Func<Task<ColorEntity>> createIfNotFound)
        {
            return base.GetOneAsync(predicate, createIfNotFound);
        }

        public override Task<bool> RemoveAsync(Expression<Func<ColorEntity, bool>> predicate)
        {
            return base.RemoveAsync(predicate);
        }

        public override Task<ColorEntity> UpdateAsync(ColorEntity entity)
        {
            return base.UpdateAsync(entity);
        }
    }
}
