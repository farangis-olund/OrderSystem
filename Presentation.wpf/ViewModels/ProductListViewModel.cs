using Infrastructure.Dtos;
using Infrastructure.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Presentation.wpf.ViewModels;

public partial class ProductListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProductService _productService;


    public ProductListViewModel(IServiceProvider serviceProvider,
                                ProductService productService,
                                Product selectedProduct,
                                ObservableCollection<Product> productList)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _selectedProduct = selectedProduct;
        _productList = productList;
        _ = LoadProducts();
    }

    [ObservableProperty]
    private ObservableCollection<Product> _productList;

    [ObservableProperty]
    private Product _selectedProduct;
        
    public async Task LoadProducts()
    {
        var products = await _productService.GetAllProduct();
                        
        ProductList.Clear();
        foreach (var product in products)
        {
            ProductList.Add(product);
        }
    }

}
