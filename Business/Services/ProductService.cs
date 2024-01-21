using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Business.Services;

public class ProductService
{
    private readonly BrandRepository _brandRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly ProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;
    public ProductService(BrandRepository brandRepository, 
                          CategoryRepository categoryRepository, 
                          ProductRepository productRepository,
                          ILogger<ProductService> logger)
    {
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<bool> CreateProduct(Product product)
    {
        try
        {
            if (await _productRepository.Exist(x => x.ArticleNumber == product.ArticleNumber))
            {
                return false;
            }
            
            var existingBrand = await _brandRepository.GetOneAsync(b => b.BrandName == product.BrandName);
            existingBrand ??= await _brandRepository.AddAsync(new BrandEntity { BrandName = product.BrandName });

            var existingCategory = await _categoryRepository.GetOneAsync(b => b.CategoryName == product.CategoryName);
            existingCategory ??= await _categoryRepository.AddAsync(new CategoryEntity { CategoryName = product.CategoryName });

            var productEntity = new ProductEntity
            {
                ArticleNumber = product.ArticleNumber,
                ProductName = product.ProductName,
                Material = product.Material,
                ProductInfo = product.ProductInfo,
                Brand = existingBrand,
                Category = existingCategory
            };

            var result = await _productRepository.AddAsync(productEntity);
            return result != null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding product: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
        }
        
    }
   
}
