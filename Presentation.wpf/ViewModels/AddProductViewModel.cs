
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
    private readonly ProductVariantService _productVariantService;
    private readonly SizeService _productSizeService;
    private readonly ProductImageService _productImageService;
    private readonly PriceService _productPriceService;
    private readonly CurrencyService _currencyService;
    public AddProductViewModel(IServiceProvider serviceProvider,
                                ProductVariantService productVariantService,
                                SizeService productSizeService,
                                ProductImageService productImageService,
                                PriceService productPriceService,
                                CurrencyService currencyService,
                                ObservableCollection<ProductSize> sizeList,
                                ProductSize selectedSize, 
                                ObservableCollection<Currency> currencyList,
                                Currency selectedCurrency)
    {
        _serviceProvider = serviceProvider;
        _productVariantService = productVariantService;
        _productSizeService = productSizeService;
        _productImageService = productImageService;
        _productPriceService = productPriceService;
        _currencyService = currencyService;
        _sizeList = sizeList;
        _selectedSize = selectedSize;
        _currencyList = currencyList;
        _selectedCurrency = selectedCurrency;
        _ = AddSizeAndCurrencyListAsync();
    }

    [ObservableProperty]
    private ProductDetail _product = new();

    [ObservableProperty]
    private ProductSize _selectedSize;

   
    [ObservableProperty]
    private ObservableCollection<ProductSize> _sizeList;

    [ObservableProperty]
    private Currency _selectedCurrency;


    [ObservableProperty]
    private ObservableCollection<Currency> _currencyList;

    [RelayCommand]
    private async Task AddProductAsync()
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

        if (SelectedCurrency == null)
        {
            MessageBox.Show("Select the currency of the product then add product!");
            return;
        } else
        {
            var currency = await _currencyService.AddCurrencyAsync(SelectedCurrency.Code, SelectedCurrency.CurrencyName);
            Product.CurrencyCode = currency.Code;

        }
        
        var productVariant = await _productVariantService.AddProductVariantAsync(Product);
        await _productImageService.AddProductImageAsync(productVariant, Product.ImageUrl);
        await _productPriceService.AddPriceAsync(productVariant, Product);
        MessageBox.Show("Product sussessfully added!");
        NavigateToProductList();
    }

    [RelayCommand]
    private void NavigateToProductList()
    {
        Product = new ProductDetail();
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        _ = productListViewModel.LoadProductsAsync();
        mainViewModel.CurrentViewModel = productListViewModel;
    }

    private async Task AddSizeAndCurrencyListAsync()
    {
        var sizes = await _productSizeService.GetAllSizesAsync();
        SizeList = new ObservableCollection<ProductSize>(sizes.Select(entity => (ProductSize)entity));

        var currencies = await _currencyService.GetAllCurrenciesAsync();
        CurrencyList = new ObservableCollection<Currency>(currencies.Select(entity => (Currency)entity));
    }
}
