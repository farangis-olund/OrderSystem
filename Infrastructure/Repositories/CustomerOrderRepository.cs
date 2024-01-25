
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CustomerOrderRepository : BaseRepository<CustomerOrderContext, CustomerOrderEntity>
{
    public CustomerOrderRepository(CustomerOrderContext context, ILogger<CustomerOrderRepository> logger)
        : base(context, logger)
    {

    }

    public override Task<CustomerOrderEntity> AddAsync(CustomerOrderEntity entity)
    {
        return base.AddAsync(entity);
    }

    public override Task<bool> Exist(Expression<Func<CustomerOrderEntity, bool>> predicate)
    {
        return base.Exist(predicate);
    }


    public override Task<IEnumerable<CustomerOrderEntity>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<CustomerOrderEntity> GetOneAsync(Expression<Func<CustomerOrderEntity, bool>> predicate, Func<Task<CustomerOrderEntity>> createIfNotFound)
    {
        return base.GetOneAsync(predicate, createIfNotFound);
    }

    public override Task<CustomerOrderEntity> GetOneAsync(Expression<Func<CustomerOrderEntity, bool>> predicate)
    {
        return base.GetOneAsync(predicate);
    }

    public override Task<bool> RemoveAsync(Expression<Func<CustomerOrderEntity, bool>> predicate)
    {
        return base.RemoveAsync(predicate);
    }

    public override Task<bool> RemoveAsync(CustomerOrderEntity entity)
    {
        return base.RemoveAsync(entity);
    }

    public override Task<CustomerOrderEntity> UpdateAsync(CustomerOrderEntity entity, Func<CustomerOrderEntity, object> keySelector)
    {
        return base.UpdateAsync(entity, keySelector);
    }
}
