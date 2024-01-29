
using Infrastructure.Entities;

namespace Infrastructure.Dtos
{
    public class OrderDetail
    {
        public int CustomerOrderId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
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
