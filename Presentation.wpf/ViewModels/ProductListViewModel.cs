using Infrastructure.Dtos;
using Infrastructure.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Presentation.wpf.Services;
using Infrastructure.Entities;


namespace Presentation.wpf.ViewModels;

public partial class ProductListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProductService _productService;
    private readonly ProductVariantService _productVariantService;
    private readonly ProductImageService _productImageService;
    private readonly PriceService _priceService;
    private readonly DataTransferService _dataTransferService = new() ;
    public ProductListViewModel(IServiceProvider serviceProvider,
                                ProductService productService,
                                ProductVariantService productVariantService,
                                ProductImageService productImageService,
                                PriceService priceService,
                                ProductDetail selectedProduct,
                                ObservableCollection<ProductDetail> productList,
                                DataTransferService dataTransferService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _productImageService = productImageService;
        _priceService = priceService;
        _selectedProduct = selectedProduct;
        _productList = productList;
        _dataTransferService = dataTransferService;
        _productVariantService = productVariantService;

        _ = LoadProductsAsync();
    }

    [ObservableProperty]
    private ObservableCollection<ProductDetail> _productList;

    [ObservableProperty]
    private ProductDetail _selectedProduct;


    public async Task LoadProductsAsync()
    {
        ProductList.Clear();

        var products = await _productVariantService.GetAllProductVariantsAsync();
        var newProduct = _dataTransferService.ConvertToProductDetails(products);
        ProductList = new ObservableCollection<ProductDetail>(newProduct);
    }


    [RelayCommand]
    private async Task RemoveProductAsync(ProductDetail product)
    {
        if (product != null)
        {
            var productVariant = await _productVariantService.GetProductVariantAsync(product);

            await _productImageService.DeleteProductImageAsync(productVariant.Id, productVariant.ArticleNumber, product.ImageUrl);
            await _priceService.DeleteProductPriceByProductVariantAsync(productVariant.Id, productVariant.ArticleNumber);
            await _productVariantService.DeleteProductVariantByArticleAsync(productVariant);
            await _productService.DeleteProductByArticleAsync(productVariant.ArticleNumber);
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
          
            _dataTransferService.SelectedProductItem = product;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductUpdateViewModel>();
        }
    }

    

}
