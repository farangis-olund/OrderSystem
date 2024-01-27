
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Repositories;

public class CustomerRepository(CustomerOrderContext context, ILogger<CustomerRepository> logger) : BaseRepository<CustomerOrderContext, CustomerEntity>(context, logger)
{
    
}
