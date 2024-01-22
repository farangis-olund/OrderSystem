using Infrastructure.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class OrderDetailEntity
{
    [Key]
    public int OrderDetailId { get; set; }

    [Required]
    public int CustomerOrderId { get; set; }

    [Required]
    public int ProductVariantId { get; set; }

    [Required]
    public int Quantity { get; set; }


    [ForeignKey(nameof(CustomerOrderId))]
    public CustomerOrderEntity CustomerOrder { get; set; } = null!;

    [ForeignKey(nameof(ProductVariantId))]
    public ProductVariantEntity ProductVariant { get; set; } = null!;

    public static implicit operator OrderDetailEntity(OrderDetail orderDetail)
    {
        return new OrderDetailEntity
        {
            CustomerOrderId = orderDetail.CustomerOrderId,
            ProductVariantId = orderDetail.ProductVariantId,
            Quantity = orderDetail.Quantity,
            CustomerOrder = orderDetail.CustomerOrder

        };
    }

}