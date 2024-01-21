using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<TContext, TEntity> : IBaseRepository<TEntity> 
    where TContext : DbContext where TEntity : class
{
    protected readonly TContext _context;
    protected readonly ILogger<BaseRepository<TContext, TEntity>> _logger;

    protected BaseRepository(TContext context, ILogger<BaseRepository<TContext, TEntity>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Entity of type {typeof(TEntity).Name} added successfully.");
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding entity of type {typeof(TEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }

    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entities of type {typeof(TEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return Enumerable.Empty<TEntity>();
        }
    }

    public virtual async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate, Func<Task<TEntity>> createIfNotFound)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);

            if (entity == null)
            {
                entity = await createIfNotFound.Invoke();
                _context.Set<TEntity>().Add(entity);
            }

            return entity;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entity of type {typeof(TEntity).Name} by id: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null;
        }
    }

    public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                return entity;
            }
            return null!;
        
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entity of type {typeof(TEntity).Name} by id: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Entity of type {typeof(TEntity).Name} updated successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating entity of type {typeof(TEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public virtual async Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity == null)
            {
                _logger.LogWarning($"Entity of type {typeof(TEntity).Name} with data {predicate} not found.");
                return false;
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Entity of type {typeof(TEntity).Name} with data {predicate} removed successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing entity of type {typeof(TEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public virtual async Task<bool> Exist(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking existence of entity of type {typeof(TEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

}
