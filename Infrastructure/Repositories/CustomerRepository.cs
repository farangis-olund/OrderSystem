
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CustomerRepository(CustomerOrderContext context, ILogger<CustomerRepository> logger) : BaseRepository<CustomerOrderContext, CustomerEntity>(context, logger)
{
    public override Task<CustomerEntity> AddAsync(CustomerEntity entity)
    {
        return base.AddAsync(entity);
    }

    public override Task<bool> Exist(Expression<Func<CustomerEntity, bool>> predicate)
    {
        return base.Exist(predicate);
    }

    public override Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<CustomerEntity?> GetOneAsync(Expression<Func<CustomerEntity, bool>> predicate, Func<Task<CustomerEntity>> createIfNotFound)
    {
        return base.GetOneAsync(predicate, createIfNotFound);
    }

    public override Task<CustomerEntity> GetOneAsync(Expression<Func<CustomerEntity, bool>> predicate)
    {
        return base.GetOneAsync(predicate);
    }

    public override Task<bool> RemoveAsync(Expression<Func<CustomerEntity, bool>> predicate)
    {
        return base.RemoveAsync(predicate);
    }

    public override Task<CustomerEntity> UpdateAsync(CustomerEntity entity)
    {
        return base.UpdateAsync(entity);
    }
}
