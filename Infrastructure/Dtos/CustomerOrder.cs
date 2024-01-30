
using Infrastructure.Entities;

namespace Infrastructure.Dtos
{
    public class CustomerOrder
    {
        public int CustomerOrderId { get; set; }
        public int TotalAmount { get; set; }
        public DateOnly Date { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; } = null!;
        public string CustomerFirstName { get; set; } = null!;
        public string CustomerLastName { get; set; } = null!;
        public string CustomerPhoneNumber { get; set; } = null!;

        public Customer Customer { get; set; } = null!;
   
        public static implicit operator CustomerOrder(CustomerOrderEntity entity)
        {
            return new CustomerOrder
            {
                CustomerOrderId = entity.Id,
                TotalAmount = entity.TotalAmount,
                Date = entity.Date,
                CustomerId = entity.CustomerId,
                Customer = entity.Customer
                
            };
        }
    }
}
