
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class OrderDetailRepository : BaseRepository<CustomerOrderContext, OrderDetailEntity>
{
    public OrderDetailRepository(CustomerOrderContext context)
        : base(context)
    {

    }

    public async override Task<IEnumerable<OrderDetailEntity>> GetAllAsync()
    {
        try
        {
            List<OrderDetailEntity> orderList = await _context.OrderDetails
            .Include(i => i.CustomerOrder).ThenInclude(i => i.Customer)
            .ToListAsync();

            return orderList;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting entities of type {typeof(OrderDetailEntity).Name}: {ex.Message}");
            return Enumerable.Empty<OrderDetailEntity>();
        }

    }

    public async override Task<OrderDetailEntity> GetOneAsync(Expression<Func<OrderDetailEntity, bool>> predicate, Func<Task<OrderDetailEntity>> createIfNotFound)
    {
        try
        {
            var entity = await _context.OrderDetails
                .Include(i => i.CustomerOrder).ThenInclude(i => i.Customer)
                .FirstOrDefaultAsync(predicate);

            entity = await createIfNotFound.Invoke();
            _context.Set<OrderDetailEntity>().Add(entity);
            return entity;

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting entity of type {typeof(OrderDetailEntity).Name} by id: {ex.Message}");
            return null!;
        }
    }
}
