
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Shared.Utils;
using System.Collections.ObjectModel;
using System.Windows;


namespace Presentation.wpf.ViewModels;

public partial class AddOrderViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CustomerOrderService _customerOrderService;
    private readonly OrderDetailService _orderDetailService;
    private readonly ProductVariantService _productVariantService;
    private readonly DtoConverter _dtoConverter;
    public AddOrderViewModel(IServiceProvider serviceProvider,
                                    CustomerOrderService customerOrderService,    
                                    OrderDetailService orderDetailService,
                                    ObservableCollection<ProductDetail> productList,
                                    ProductVariantService productVariantService,
                                    DtoConverter dtoConverter,
                                    ProductDetail selectedProduct)
    {

        _serviceProvider = serviceProvider;
        _customerOrderService = customerOrderService;
        _orderDetailService = orderDetailService;
        _productVariantService = productVariantService;
        _productList = productList;
        _dtoConverter = dtoConverter;
        _selectedProduct = selectedProduct;

        _ = LoadProductsAsync();
    }

    [ObservableProperty]
    private ObservableCollection<ProductDetail> _productList;
    
    [ObservableProperty]
    private Customer _customer = new();

    [ObservableProperty]
    private ProductDetail _selectedProduct;

    [RelayCommand]
    private void CreateOrder()
    {
       NavigateToOrderList();
    }

    private async Task AddOrder()
    {
        CustomerOrder newCustomerOrder = new();
        if (Customer != null)
        {
            if (Customer.FirstName != null && Customer.LastName != null & Customer.Email != null)
            {
                newCustomerOrder.CustomerFirstName = Customer.FirstName;
                newCustomerOrder.CustomerLastName = Customer.LastName!;
                newCustomerOrder.CustomerEmail = Customer.Email!;
                newCustomerOrder.CustomerPhoneNumber = Customer.PhoneNumber!;
                newCustomerOrder.Date = DateOnly.FromDateTime(DateTime.Now);
                newCustomerOrder.TotalAmount = (int)ProductList
                    .Where(p => p.OrderQuantity != 0)
                    .Sum(product => product.Price * product.OrderQuantity);
            }
        }

        var customerOrder = await _customerOrderService.AddCustomerOrderAsync(newCustomerOrder);

        foreach (var product in ProductList.Where(p => p.OrderQuantity != 0))
        {
            var productVariangt = await _productVariantService.GetProductVariantAsync(product);
            var Order = new OrderDetail
            {
                CustomerOrderId = customerOrder.Id,
                ProductVariantId = productVariangt.Id,
                Quantity = product.OrderQuantity
            };
            
            await _orderDetailService.AddOrderDetailAsync(Order);
        }
        
    }

    public async Task LoadProductsAsync()
    {
        ProductList.Clear();

        var products = await _productVariantService.GetAllProductVariantsAsync();
        var newProduct = _dtoConverter.ConvertToProductDetails(products);
        ProductList = new ObservableCollection<ProductDetail>(newProduct);
    }

    private async void NavigateToOrderList()
    {   
        await AddOrder();
        MessageBox.Show("Order sucessfully added!");
        Customer = new Customer();
        _ = LoadProductsAsync();
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var orderListViewModel = _serviceProvider.GetRequiredService<OrderListViewModel>();
        mainViewModel.CurrentViewModel = orderListViewModel;
    }
}
