
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<TContext, TEntity> where TContext : DbContext where TEntity : class  
{
    protected readonly TContext _context;
    protected readonly ILogger<BaseRepository<TContext, TEntity>> _logger;

    protected BaseRepository(TContext context, ILogger<BaseRepository<TContext, TEntity>> logger)
    {
        _context = context;
        _logger = logger;   
    }

    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Entity of type {typeof(TEntity).Name} added successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding entity of type {typeof(TEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
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

    public virtual async Task<TEntity?> GetByIdAsync(object id)
    {
        try
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entity of type {typeof(TEntity).Name} by id: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null;
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

    public virtual async Task<bool> RemoveAsync(object id)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                _logger.LogWarning($"Entity of type {typeof(TEntity).Name} with id {id} not found.");
                return false;
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Entity of type {typeof(TEntity).Name} with id {id} removed successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing entity of type {typeof(TEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }


}
