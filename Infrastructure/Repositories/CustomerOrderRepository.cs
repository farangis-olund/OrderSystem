
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CustomerOrderRepository : BaseRepository<CustomerOrderContext, CustomerOrderEntity>
{
    public CustomerOrderRepository(CustomerOrderContext context)
        : base(context)
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
            Debug.WriteLine($"Error getting entities of type {typeof(CustomerOrderEntity).Name}: {ex.Message}");
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
            Debug.WriteLine($"Error getting entity of type {typeof(CustomerOrderEntity).Name} by id: {ex.Message}");
            return null!;
        }
    }
}
