using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}