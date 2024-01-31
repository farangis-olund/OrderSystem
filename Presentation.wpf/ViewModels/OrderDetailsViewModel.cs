
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.wpf.Services;
using Shared.Utils;
using System.Collections.ObjectModel;


namespace Presentation.wpf.ViewModels
{
    public partial class OrderDetailsViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly OrderDetailService _orderDetailService;
        private readonly ProductVariantService _productVariantService;
        private readonly DataTransferService _dataTransferService;
        private readonly DtoConverter _dtoConverter;
        public OrderDetailsViewModel(IServiceProvider serviceProvider,
                                        OrderDetailService orderDetailService,
                                        ObservableCollection<ProductDetail> productList,
                                        ProductVariantService productVariantService,
                                        DataTransferService dataTransferService,
                                        DtoConverter dtoConverter,
                                        ProductDetail selectedProduct)
        {

            _serviceProvider = serviceProvider;
            _orderDetailService = orderDetailService;
            _productVariantService = productVariantService;
            _productList = productList;
            _dataTransferService = dataTransferService;
            _dtoConverter = dtoConverter;
            _selectedProduct = selectedProduct;

            _ = LoadProductsAsync();

        }

        [ObservableProperty]
        private ObservableCollection<ProductDetail> _productList;



        [ObservableProperty]
        private CustomerOrder _customerOrder = new();

        [ObservableProperty]
        private ProductDetail _selectedProduct;

        [RelayCommand]
        private void NavigateToOrder()
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<OrderListViewModel>();
        }

        public async Task LoadProductsAsync()
        {
            ProductList.Clear();
            var customerOrder = _dataTransferService.SelectedOrderItem;
            CustomerOrder = customerOrder;
            CustomerOrder.Customer = customerOrder.Customer;

            var orderDetailProducts = await _orderDetailService.GetAllOrderDetailsAsync();

            var orderDetailsSet = new HashSet<int>(orderDetailProducts
                .Where(item => item.CustomerOrderId == customerOrder.CustomerOrderId)
                .Select(item => item.ProductVariantId));

            var productVariantList = await _productVariantService.GetAllProductVariantsAsync();

            var selectedItems = productVariantList
                .Where(item => orderDetailsSet.Contains(item.Id))
                .ToList();

            foreach (var selectedItem in selectedItems)
            {
                ProductList.Add(_dtoConverter.ConvertToProductDetail(selectedItem));
            }

            var productDictionary = ProductList.ToDictionary(p => p.ProductVariantId);

            foreach (var orderDetail in orderDetailProducts)
            {
                if (productDictionary.TryGetValue(orderDetail.ProductVariantId, out var product))
                {
                    product.OrderQuantity = orderDetail.Quantity;
                }
            }
        }



    }
}
