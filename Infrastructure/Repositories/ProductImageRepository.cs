
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ProductImageRepository : BaseRepository<ProductDataContext, ProductImageEntity>
{
    public ProductImageRepository(ProductDataContext context, ILogger<ProductImageRepository> logger)
        : base(context, logger)
    {

    }

    public async override Task<IEnumerable<ProductImageEntity>> GetAllAsync()
    {
        try
        {
            List<ProductImageEntity> productImageList = await _context.ProductImageEntities
            .Include(i => i.Image)
            .ToListAsync();

            return productImageList;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entities of type {typeof(ProductImageEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return Enumerable.Empty<ProductImageEntity>();
        }
    }

    public async override Task<ProductImageEntity> GetOneAsync(Expression<Func<ProductImageEntity, bool>> predicate, Func<Task<ProductImageEntity>> createIfNotFound)
    {
        try
        {
            var entity = await _context.ProductImageEntities
                 .Include(i => i.Image)
                 .FirstOrDefaultAsync(predicate);

            entity = await createIfNotFound.Invoke();
            _context.Set<ProductImageEntity>().Add(entity);
            return entity;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entity of type {typeof(ProductImageEntity).Name} by id: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

}
