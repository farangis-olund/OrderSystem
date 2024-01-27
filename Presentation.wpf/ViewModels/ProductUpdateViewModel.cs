
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
    private readonly SizeService _productSizeService;
    private readonly ProductImageService _productImageService;
    private readonly PriceService _productPriceService;
    private readonly ColorService _productColorService;
    private readonly CategoryService _categoryService;
    private readonly BrandService _brandService;
    private readonly DataTransferService _dataTransferService;
    public ProductUpdateViewModel(IServiceProvider serviceProvider, 
                                  ProductService productService, 
                                  ProductVariantService productVariantService, 
                                  SizeService productSizeService, 
                                  ProductImageService productImageService, 
                                  PriceService productPriceService,
                                  ColorService productColorService,
                                  CategoryService categoryService,
                                  BrandService brandService,
                                  DataTransferService dataTransferService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _productVariantService = productVariantService;
        _productSizeService = productSizeService;
        _productImageService = productImageService;
        _productPriceService = productPriceService;
        _productColorService = productColorService;
        _categoryService = categoryService;
        _brandService = brandService;
        _dataTransferService = dataTransferService;
           
         Product = _dataTransferService.SelectedProductItem;
        

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
                var productVariant = await _productVariantService.GetProductVariantAsync(Product);
                var color = await _productColorService.GetColorAsync(Product.ColorName);
                var size = await _productSizeService.GetSizeAsync(Product.Size);
                var brand = await _brandService.GetBrandAsync(Product.BrandName);
                var category = await _categoryService.GetCategoryAsync(Product.CategoryName);

                productVariant.Quantity = Product.Quantity;
                productVariant.SizeId = size.Id;
                productVariant.ColorId =color.Id;
                Product.BrandId = brand.Id;
                Product.CategoryId = category.Id;

                await _productImageService.UpdateProductImageAsync(productVariant, Product.ImageUrl);
                await _productPriceService.UpdateProductPriceByProductVariantAsync(productVariant.Id, productVariant.ArticleNumber, Product.Price);
                
                await _productVariantService.UpdateProductVariantAsync(productVariant);
                await _productService.UpdateProductAsync(Product);

                MessageBox.Show("Product sussessfully updated!");
                NavigateToProductList();
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
