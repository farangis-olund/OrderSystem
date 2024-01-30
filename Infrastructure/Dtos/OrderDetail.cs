
using Infrastructure.Entities;

namespace Infrastructure.Dtos
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int CustomerOrderId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateOnly Date { get; set; }
        public int TotalAmount { get; set; }
        public Customer Customer { get; set; } = null!;
        public CustomerOrder CustomerOrder { get; set; } = null!;
        public ProductDetail ProductDetail { get; set; } = null!;

        public static implicit operator OrderDetail(OrderDetailEntity entity)
        {
            return new OrderDetail
            {
                CustomerOrderId = entity.CustomerOrderId,
                ProductVariantId = entity.ProductVariantId,
                Quantity = entity.Quantity,
                CustomerOrder = entity.CustomerOrder
            };
        }

    }
}
