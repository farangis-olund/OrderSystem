
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class OrderDetailRepository : BaseRepository<CustomerOrderContext, OrderDetailEntity>
{
    public OrderDetailRepository(CustomerOrderContext context, ILogger<OrderDetailRepository> logger)
        : base(context, logger)
    {

    }

    public async override Task<IEnumerable<OrderDetailEntity>> GetAllAsync()
    {
        try
        {
            List<OrderDetailEntity> productList = await _context.OrderDetails
            .Include(x => x.CustomerOrder)
            .Include (s => s.ProductVariant)
            .ToListAsync();

            return productList;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entities of type {typeof(OrderDetailEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return Enumerable.Empty<OrderDetailEntity>();
        }

    }

    public async override Task<OrderDetailEntity> GetOneAsync(Expression<Func<OrderDetailEntity, bool>> predicate, Func<Task<OrderDetailEntity>> createIfNotFound)
    {
        try
        {
            var entity = await _context.OrderDetails
                .Include(x => x.CustomerOrder)
                .Include(s => s.ProductVariant)
                .FirstOrDefaultAsync(predicate);

            entity = await createIfNotFound.Invoke();
            _context.Set<OrderDetailEntity>().Add(entity);
            return entity;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entity of type {typeof(OrderDetailEntity).Name} by id: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }
}
