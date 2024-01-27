
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CustomerOrderRepository : BaseRepository<CustomerOrderContext, CustomerOrderEntity>
{
    public CustomerOrderRepository(CustomerOrderContext context, ILogger<CustomerOrderRepository> logger)
        : base(context, logger)
    {

    }

    public async override Task<IEnumerable<CustomerOrderEntity>> GetAllAsync()
    {
        try
        {
            List<CustomerOrderEntity> customerOrderList = await _context.CustomerOrders
            .Include(i => i.Customer)
            .ToListAsync();

            return customerOrderList;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entities of type {typeof(CustomerOrderEntity).Name}: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return Enumerable.Empty<CustomerOrderEntity>();
        }

    }

    public async override Task<CustomerOrderEntity> GetOneAsync(Expression<Func<CustomerOrderEntity, bool>> predicate, Func<Task<CustomerOrderEntity>> createIfNotFound)
    {
        try
        {
            var entity = await _context.CustomerOrders
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(predicate);

            entity = await createIfNotFound.Invoke();
            _context.Set<CustomerOrderEntity>().Add(entity);
            return entity;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting entity of type {typeof(CustomerOrderEntity).Name} by id: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }
}
