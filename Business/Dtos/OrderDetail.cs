
namespace Business.Dtos
{
    public class OrderDetail
    {
        public int CustomerOrderId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }

        public CustomerOrder CustomerOrder { get; set; } = null!;
        public ProductVariant ProductVariant { get; set; } = null!;
       
 
    }
}
