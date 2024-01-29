
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class PriceRepository : BaseRepository<ProductDataContext, ProductPriceEntity>
{
    public PriceRepository(ProductDataContext context)
        : base(context)
    {
      
    }

    public async override Task<IEnumerable<ProductPriceEntity>> GetAllAsync()
    {
        try
        {
            List<ProductPriceEntity> productPriceList = await _context.ProductPriceEntities
            .Include(i => i.CurrencyCodeNavigation)
            .ToListAsync();

            return productPriceList;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting entities of type {typeof(ProductPriceEntity).Name}: {ex.Message}");
            return Enumerable.Empty<ProductPriceEntity>();
        }
    }

    public async override Task<ProductPriceEntity> GetOneAsync(Expression<Func<ProductPriceEntity, bool>> predicate, Func<Task<ProductPriceEntity>> createIfNotFound)
    {
        try
        {
            var entity = await _context.ProductPriceEntities
                 .Include(i => i.CurrencyCodeNavigation)
                 .FirstOrDefaultAsync(predicate);

            entity = await createIfNotFound.Invoke();
            _context.Set<ProductPriceEntity>().Add(entity);
            return entity;

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting entity of type {typeof(ProductPriceEntity).Name} by id: {ex.Message}");
            return null!;
        }
    }
    
}
