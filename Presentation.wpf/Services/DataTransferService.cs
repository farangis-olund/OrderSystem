using Infrastructure.Dtos;

namespace Presentation.wpf.Services;

public class DataTransferService
{
    public ProductDetail SelectedProductItem { get; set; } = null!;
    public Customer SelectedCustomerItem { get; set; } = null!;
    public CustomerOrder SelectedOrderItem { get; set; } = null!;
}