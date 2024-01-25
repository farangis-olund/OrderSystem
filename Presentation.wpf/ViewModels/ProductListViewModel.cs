using Infrastructure.Dtos;
using Infrastructure.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Presentation.wpf.Services;


namespace Presentation.wpf.ViewModels;

public partial class ProductListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProductService _productService;
    private readonly ProductPresentationService _productPresentationService;
    private readonly DataTransferService _dataTransferService;
    public ProductListViewModel(IServiceProvider serviceProvider,
                                ProductService productService,
                                ProductPresentationService productPresentationService,
                                ProductDetail selectedProduct,
                                ObservableCollection<ProductDetail> productList,
                                DataTransferService dataTransferService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _selectedProduct = selectedProduct;
        _productList = productList;
        _dataTransferService = dataTransferService;
        _productPresentationService = productPresentationService;
        
        _ = LoadProductsAsync();
    }

    [ObservableProperty]
    private ObservableCollection<ProductDetail> _productList;

    [ObservableProperty]
    private ProductDetail _selectedProduct;

    
    public async Task LoadProductsAsync()
    {
        ProductList.Clear();
        
        var products = await _productPresentationService.GetAllProductDetailsAsync();
        ProductList = new ObservableCollection<ProductDetail>(products);
    }
    [RelayCommand]
    private async Task RemoveProductAsync(ProductDetail product)
    {
        if (product != null)
        {
            await _productService.DeleteProductByArticleAsync(product.ArticleNumber);
            ProductList.Remove(product);
        }
       _ = LoadProductsAsync();
    }
    
    [RelayCommand]
    private void NavigateToAddProduct()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AddProductViewModel>();
    }

    [RelayCommand]
    private void NavigateToUpdate(ProductDetail product)
    {
        if (product != null)
        {
            _dataTransferService.selectedProduct = product;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductUpdateViewModel>();
        }
    }

}
