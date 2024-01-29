
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.wpf.Services;
using System.Collections.ObjectModel;

namespace Presentation.wpf.ViewModels
{
    public partial class OrderListViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly OrderDetailService _orderDetailService;
        private readonly DataTransferService _transferService;

        public OrderListViewModel(IServiceProvider serviceProvider,
                                    OrderDetailService orderDetailService,
                                    ObservableCollection<OrderDetail> orderList,
                                    DataTransferService transferService)
        {
            
            _serviceProvider = serviceProvider;
            _orderDetailService = orderDetailService;
            _orderList = orderList;
            _transferService = transferService;
            _ = LoadOrdersAsync();
        }

        [ObservableProperty]
        private ObservableCollection<OrderDetail> _orderList;

        [RelayCommand]
        private async Task RemoveOrderAsync(OrderDetail order)
        {
            if (order != null)
            {
                await _orderDetailService.DeleteOrderDetailAsync(order);
            }
            _ = LoadOrdersAsync();
        }

        [RelayCommand]
        private void NavigateToAddOrder()
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AddCustomerViewModel>();
        }

        [RelayCommand]
        private void NavigateToUpdate(OrderDetail order)
        {
            if (order != null)
            {
                _transferService.SelectedOrderItem = order;
                var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
                mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UpdateCustomerViewModel>();
            }
        }
        public async Task LoadOrdersAsync()
        {
            OrderList.Clear();

            var orders = await _orderDetailService.GetAllOrderDetailsAsync();
            var newProduct = _transferService.ConvertToOrderDetails(orders);
            OrderList = new ObservableCollection<OrderDetail>(newProduct);
        }
    }
}
