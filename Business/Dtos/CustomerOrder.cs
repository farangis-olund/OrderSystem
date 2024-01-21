
namespace Business.Dtos
{
    public class CustomerOrder
    {
        public int Id { get; set; }
        public int TotalAmount { get; set; }
        public DateOnly Date { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
        public List<OrderDetail> OrderDetails { get; set; } = [];
    }
}
