
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation.wpf.ViewModels;

public partial class AddProductViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProductService _productService;
    private readonly ProductVariantService _productVariantService;
    private readonly ProductSizeService _productSizeService;
    private readonly ProductImageService _productImageService;
    private readonly ProductPriceService _productPriceService;
    public AddProductViewModel(IServiceProvider serviceProvider,
                                ProductService productService,
                                ProductVariantService productVariantService,
                                ProductSizeService productSizeService,
                                ProductImageService productImageService,
                                ProductPriceService productPriceService,
                                ObservableCollection<ProductSize> sizeList,
                                ProductSize selectedSize)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _productVariantService = productVariantService;
        _productSizeService = productSizeService;
        _productImageService = productImageService;
        _productPriceService = productPriceService;
        _sizeList = sizeList;
        _selectedSize = selectedSize;
        _ = AddSizeList();
    }

    [ObservableProperty]
    private ProductDetail _product = new();

    [ObservableProperty]
    private ProductSize _selectedSize;

   
    [ObservableProperty]
    private ObservableCollection<ProductSize> _sizeList;
      
    [RelayCommand]
    private async Task AddProduct()
    {
        if (SelectedSize == null)
        {
            MessageBox.Show("Select the size of the product then add product!");
            return;
        }

        var size = await _productSizeService.GetSizeAsync(SelectedSize);

        if (size == null)
        {
            size = await _productSizeService.AddSizeAsync(SelectedSize);
            var sizeId = size.Id;
            Product.SizeId = sizeId;
        }
        else
        {
            Product.SizeId = size.Id;
        }

        await _productService.AddProductAsync(Product);
        var newProductVariant = await _productVariantService.AddProductVariantAsync(Product);
        
        var newProductImage = new ProductImage
        {
            ProductVariantId = newProductVariant.Id,
            ArticleNumber = newProductVariant.ArticleNumber,
            ImageUrl = Product.ImageUrl
        };
        await _productImageService.AddProductImageAsync(newProductImage);

        var newProductPrice = new ProductPrice
        {
            ProductVariantId = newProductVariant.Id,
            ArticleNumber = newProductVariant.ArticleNumber,
            Price = Product.Price,
            Code = Product.Currency
        };
        await _productPriceService.AddProductPriceAsync(newProductPrice);
        MessageBox.Show("Product sussessfully added!");
        NavigateToProductList();
    }

    [RelayCommand]
    private void NavigateToProductList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        _ = productListViewModel.LoadProductsAsync();
        mainViewModel.CurrentViewModel = productListViewModel;
    }

    private async Task AddSizeList()
    {
        var sizes = await _productSizeService.GetAllSizesAsync() ;
        SizeList = new ObservableCollection<ProductSize>(sizes.Select(entity => (ProductSize)entity));
    }
}
