
using Infrastructure.Entities;

namespace Business.Dtos
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public List<CustomerOrderEntity> CustomerOrders { get; set; } = [];
    }
}
