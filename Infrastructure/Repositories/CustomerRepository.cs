
using Infrastructure.Contexts;
using Infrastructure.Entities;


namespace Infrastructure.Repositories;

public class CustomerRepository(CustomerOrderContext context) : BaseRepository<CustomerOrderContext, CustomerEntity>(context)
{
    
}
