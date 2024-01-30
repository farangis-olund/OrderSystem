using Infrastructure.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class CustomerOrderEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TotalAmount { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    [ForeignKey(nameof(CustomerEntity))]
    public Guid CustomerId { get; set; }


    [ForeignKey("CustomerId")]
    [InverseProperty("CustomerOrders")]
    public virtual CustomerEntity Customer { get; set; } = null!;

    public virtual ICollection<OrderDetailEntity> OrderDetails { get; set; } = new HashSet<OrderDetailEntity>();

    public static implicit operator CustomerOrderEntity(CustomerOrder customerOrder)
    {
        return new CustomerOrderEntity
        {
            Id = customerOrder.CustomerOrderId,
            TotalAmount = customerOrder.TotalAmount,
            Date = customerOrder.Date,
            CustomerId = customerOrder.CustomerId,
            Customer = customerOrder.Customer


        };
    }

}
