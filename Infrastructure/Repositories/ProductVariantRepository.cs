using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ProductVariantRepository : BaseRepository<ProductDataContext, ProductVariantEntity>
{
    public ProductVariantRepository(ProductDataContext context, ILogger<ProductVariantRepository> logger)
        : base(context, logger)
    {

    }

    public async override Task<IEnumerable<ProductVariantEntity>> GetAllAsync()
    {
        try
        {
            List<ProductVariantEntity> productVariantList = await _context.ProductVariantEntities
               .Include(i => i.ArticleNumberNavigation)
               .Include(i => i.Color)
               .Include(i => i.Size)
               .Include(i => i.ArticleNumberNavigation.Brand)
               .Include(i => i.ArticleNumberNavigation.Category)
               .Include(i => i.ProductImageEntities)
                   .ThenInclude(pi => pi.Image)
               .Include(i => i.ProductPriceEntities)
                  .ThenInclude(pp => pp.CurrencyCodeNavigation)
               .ToListAsync();

            return productVariantList;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entities of type {typeof(ProductVariantEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return Enumerable.Empty<ProductVariantEntity>();
        }
    }

    public async override Task<ProductVariantEntity> GetOneAsync(Expression<Func<ProductVariantEntity, bool>> predicate, Func<Task<ProductVariantEntity>> createIfNotFound)
    {
        try
        {
            var entity = await _context.ProductVariantEntities
               .Include(i => i.Color)
               .Include(i => i.Size)
               .FirstOrDefaultAsync(predicate);

            entity = await createIfNotFound.Invoke();
            _context.Set<ProductVariantEntity>().Add(entity);
            return entity;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entity of type {typeof(ProductVariantEntity).Name} by id: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }
}
