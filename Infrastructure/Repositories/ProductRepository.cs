
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ProductRepository : BaseRepository<ProductDataContext, ProductEntity>
{
    new private readonly ProductDataContext _context;
    public ProductRepository(ProductDataContext context, ILogger<BaseRepository<ProductDataContext, ProductEntity>> logger) 
        : base(context, logger)
    {
        _context = context;
    }

    public override async Task<IEnumerable<ProductEntity>> GetAllAsync()
    {
        try
        {
            List<ProductEntity> productList = await _context.ProductEntities
            .Include(i => i.Brand)
            .Include(i => i.Category)
            .ToListAsync();

            return productList;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entities of type {typeof(ProductEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return Enumerable.Empty<ProductEntity>();
        }
       

    }

    public override async Task<ProductEntity> GetOneAsync(Expression<Func<ProductEntity, bool>> predicate, Func<Task<ProductEntity>> createIfNotFound)
    {
        try
        {
            var entity = await _context.ProductEntities
                .Include (i => i.Brand)
                .Include(i => i.Category)
                .FirstOrDefaultAsync(predicate);

                entity = await createIfNotFound.Invoke();
                _context.Set<ProductEntity>().Add(entity);
             return entity;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entity of type {typeof(ProductEntity).Name} by id: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

   
}
