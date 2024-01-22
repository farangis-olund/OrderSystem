
using Infrastructure.Entities;

namespace Infrastructure.Dtos
{
    public class CustomerOrder
    {
        public int TotalAmount { get; set; }
        public DateOnly Date { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; } = null!;
        public Customer Customer { get; set; } = null!;

        public static implicit operator CustomerOrder(CustomerOrderEntity entity)
        {
            return new CustomerOrder
            {
                TotalAmount = entity.TotalAmount,
                Date = entity.Date,
                CustomerId = entity.CustomerId
              
            };
        }
    }
}
