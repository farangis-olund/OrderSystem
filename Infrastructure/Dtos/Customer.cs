
using Infrastructure.Entities;

namespace Infrastructure.Dtos
{
    public class Customer
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public List<CustomerOrder> Orders { get; set; } = [];

        public static implicit operator Customer(CustomerEntity entity)
        {
            return new Customer
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
               
            };
        }

    }
}
