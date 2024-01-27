
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Presentation.wpf.ViewModels;

public partial class AddCustomerViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CustomerService _customerService;
   
    public AddCustomerViewModel(IServiceProvider serviceProvider,
                                CustomerService customerService)
    {
        _serviceProvider = serviceProvider;
        _customerService = customerService;
       
    }

    [ObservableProperty]
    private Customer _customer = new();
    

    [RelayCommand]
    private async Task AddCustomerAsync()
    {
        if (Customer != null)
        {
            await _customerService.AddCustomerAsync(Customer);
        }
                
        MessageBox.Show("Customer sussessfully added!");
        NavigateToCustomerList();
    }

    [RelayCommand]
    private void NavigateToCustomerList()
    {
        Customer = new Customer();
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var customerListViewModel = _serviceProvider.GetRequiredService<CustomerListViewModel>();
        _ = customerListViewModel.LoadCustomersAsync();
        mainViewModel.CurrentViewModel = customerListViewModel;
    }

}
