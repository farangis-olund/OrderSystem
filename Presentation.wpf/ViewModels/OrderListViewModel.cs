
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
    public partial class OrderListViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CustomerOrderService _customerOrderService;
        private readonly OrderDetailService _orderDetailService;
        private readonly DataTransferService _transferService;
     

        public OrderListViewModel(IServiceProvider serviceProvider,
                                    CustomerOrderService customerOrderService,
                                    OrderDetailService orderDetailService,
                                    ObservableCollection<CustomerOrder> orderList,
                                    DataTransferService transferService)
        {
            
            _serviceProvider = serviceProvider;
            _customerOrderService = customerOrderService;
            _orderDetailService = orderDetailService;
            _orderList = orderList;
           
            _transferService = transferService;
            _ = LoadOrdersAsync();
        }

        [ObservableProperty]
        private ObservableCollection<CustomerOrder> _orderList;

       

        [RelayCommand]
        private async Task RemoveOrderAsync(CustomerOrder order)
        {
            if (order != null)
            {
                await _orderDetailService.DeleteOrderDetailByOrderIdAsync(order.CustomerOrderId);
                await _customerOrderService.DeleteCustomerOrderByIdAsync(order.CustomerOrderId);
            }
            _ = LoadOrdersAsync();
        }

        [RelayCommand]
        private void NavigateToAddOrder()
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AddOrderViewModel>();
        }

        [RelayCommand]
        private void NavigateToDetail(CustomerOrder order)
        {
            if (order != null)
            {
                _transferService.SelectedOrderItem = order;
                var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
                mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<OrderDetailsViewModel>();
            }
        }
        public async Task LoadOrdersAsync()
        {
            OrderList.Clear();
            var orders = await _customerOrderService.GetAllCustomerOrdersAsync();
            var newOrder = DtoConverter.ConvertToOrderDetails(orders);
            OrderList = new ObservableCollection<CustomerOrder>(newOrder);
        }
    }
}
