
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Presentation.wpf.Services;

namespace Presentation.wpf.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DataTransferService _dataTransferService;

    [ObservableProperty]
    private ObservableObject _currentViewModel = null!;

    public MainViewModel(IServiceProvider serviceProvider, DataTransferService dataTransferService)
    {
        _serviceProvider = serviceProvider;
        _dataTransferService = dataTransferService;
               
        NavigateToInitialView();
    }
        
    private void NavigateToInitialView()
    {
        CurrentViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
    }

    [RelayCommand]
    private void NavigateToProductsList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
    }

    [RelayCommand]
    private void NavigateCustomersList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CustomerListViewModel>();
    }
}