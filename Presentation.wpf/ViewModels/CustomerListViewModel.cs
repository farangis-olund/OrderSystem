
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.wpf.Services;
using System.Collections.ObjectModel;

namespace Presentation.wpf.ViewModels;

public partial class CustomerListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CustomerService _customerService;
    private readonly DataTransferService _dataTransferService = new();
    public CustomerListViewModel(IServiceProvider serviceProvider,
                                Customer selectedCustomer,
                                CustomerService customerService,
                                ObservableCollection<Customer> customerList,
                                DataTransferService dataTransferService)
    {
        _serviceProvider = serviceProvider;
        _customerService = customerService;
        _selectedCustomer = selectedCustomer;
        _customerList = customerList;
        _dataTransferService = dataTransferService;
       

        _ = LoadCustomersAsync();
    }

    [ObservableProperty]
    private ObservableCollection<Customer> _customerList;

    [ObservableProperty]
    private Customer _selectedCustomer;


    [RelayCommand]
    private async Task RemoveCustomerAsync(Customer customer)
    {
        if (customer != null)
        {
           await _customerService.DeleteCustomerAsync(customer.Email);
        }
        _ = LoadCustomersAsync();
    }

    [RelayCommand]
    private void NavigateToAddCustomer()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AddCustomerViewModel>();
    }

    [RelayCommand]
    private void NavigateToUpdate(Customer customer)
    {
        if (customer != null)
        {

            _dataTransferService.SelectedCustomerItem = customer;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UpdateCustomerViewModel>();
        }
    }

        public async Task LoadCustomersAsync()
    {
        CustomerList.Clear();

        var customers = await _customerService.GetAllCustomersAsync();

        CustomerList = new ObservableCollection<Customer>(customers);

    }

}
