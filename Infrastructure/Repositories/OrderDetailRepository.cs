
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class OrderDetailRepository : BaseRepository<CustomerOrderContext, OrderDetailEntity>
{
    public OrderDetailRepository(CustomerOrderContext context, ILogger<OrderDetailRepository> logger)
        : base(context, logger)
    {

    }

    public override Task<OrderDetailEntity> AddAsync(OrderDetailEntity entity)
    {
        return base.AddAsync(entity);
    }

    public override Task<bool> Exist(Expression<Func<OrderDetailEntity, bool>> predicate)
    {
        return base.Exist(predicate);
    }

    public override Task<IEnumerable<OrderDetailEntity>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<OrderDetailEntity?> GetOneAsync(Expression<Func<OrderDetailEntity, bool>> predicate, Func<Task<OrderDetailEntity>> createIfNotFound)
    {
        return base.GetOneAsync(predicate, createIfNotFound);
    }

    public override Task<OrderDetailEntity> GetOneAsync(Expression<Func<OrderDetailEntity, bool>> predicate)
    {
        return base.GetOneAsync(predicate);
    }

    public override Task<bool> RemoveAsync(Expression<Func<OrderDetailEntity, bool>> predicate)
    {
        return base.RemoveAsync(predicate);
    }

    public override Task<OrderDetailEntity> UpdateAsync(OrderDetailEntity entity)
    {
        return base.UpdateAsync(entity);
    }
}
