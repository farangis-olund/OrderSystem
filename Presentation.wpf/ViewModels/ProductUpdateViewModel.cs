
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.wpf.Services;
using System.Diagnostics;
using System.Windows;

namespace Presentation.wpf.ViewModels;

public partial class ProductUpdateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProductService _productService ;
    private readonly ProductVariantService _productVariantService ;
    private readonly ProductSizeService _productSizeService;
    private readonly ProductImageService _productImageService;
    private readonly ProductPriceService _productPriceService;
    private readonly ProductPresentationService _productPresentationService;
    private readonly DataTransferService _dataTransferService;
      

    public ProductUpdateViewModel(IServiceProvider serviceProvider, 
                                  ProductService productService, 
                                  ProductVariantService productVariantService, 
                                  ProductSizeService productSizeService, 
                                  ProductImageService productImageService, 
                                  ProductPriceService productPriceService,
                                  ProductPresentationService productPresentationService,
                                  DataTransferService dataTransferService
                                  )
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _productVariantService = productVariantService;
        _productSizeService = productSizeService;
        _productImageService = productImageService;
        _productPriceService = productPriceService;
        _productPresentationService = productPresentationService;
        _dataTransferService = dataTransferService;
        
        Product = _dataTransferService.selectedProduct;
        

    }
    
    [ObservableProperty]
    private ProductDetail _product = new();
    
    [RelayCommand]
    private async Task UpdateProduct()
    {
        try
        {
            if (Product != null)
            {
                await _productPresentationService.UpdateProductDetailAsync(Product);
                MessageBox.Show("Product sussessfully updated!");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

    }

    [RelayCommand]
    private void NavigateToProductList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        _ = productListViewModel.LoadProductsAsync();
        mainViewModel.CurrentViewModel = productListViewModel;
    }
}
