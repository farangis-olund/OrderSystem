using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> UpdateAsync(TEntity entity, Func<TEntity, object> keySelector);
        Task<bool> Exist(Expression<Func<TEntity, bool>> predicate);
    }
}