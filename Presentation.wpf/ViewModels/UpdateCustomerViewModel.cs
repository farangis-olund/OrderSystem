
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.wpf.Services;
using System.Diagnostics;
using System.Windows;

namespace Presentation.wpf.ViewModels;

public partial class UpdateCustomerViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CustomerService _customerService;
    private readonly DataTransferService _dataTransferService;
    public UpdateCustomerViewModel(IServiceProvider serviceProvider,
                                 CustomerService customerService,
                                 DataTransferService dataTransferService)
    {
        _serviceProvider = serviceProvider;
        _customerService = customerService;
        _dataTransferService = dataTransferService;

        Customer = _dataTransferService.SelectedCustomerItem;


    }

    [ObservableProperty]
    private Customer _customer = new();

    [RelayCommand]
    private async Task UpdateCustomer()
    {
        try
        {
            if (Customer != null)
            {
                await _customerService.UpdateCustomerAsync(Customer);
               
                MessageBox.Show("Customer sussessfully updated!");
                NavigateToCustomerList();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

    }

    [RelayCommand]
    private void NavigateToCustomerList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<CustomerListViewModel>();
        _ = productListViewModel.LoadCustomersAsync();
        mainViewModel.CurrentViewModel = productListViewModel;
    }
}
