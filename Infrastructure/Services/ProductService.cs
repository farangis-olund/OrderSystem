using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;
    private readonly BrandService _brandService;
    private readonly CategoryService _categoryService;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ProductRepository productRepository,
                          BrandService brandService,
                          CategoryService categoryService,
                          ILogger<ProductService> logger)
    {
        
        _productRepository = productRepository;
        _brandService = brandService;
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<ProductEntity> AddProductAsync(Product product)
    {
        try
        {
            var brandEntity = await _brandService.AddBrandAsync(product.BrandName);
            var categoryEntity = await _categoryService.AddCategoryAsync(product.CategoryName);

            var existingProduct = await _productRepository.GetOneAsync(p => p.ArticleNumber == product.ArticleNumber);
            if (existingProduct != null)
            {
                return null!;
            }
            var productEntity = new ProductEntity
            {
                ArticleNumber = product.ArticleNumber,
                ProductName = product.ProductName,
                Material = product.Material,
                ProductInfo = product.ProductInfo,
                BrandId = brandEntity.Id,
                CategoryId = categoryEntity.Id
            };

            return await _productRepository.AddAsync(productEntity);
           
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding product: {ex.Message}");
            Debug.WriteLine($"Error getting product: {ex.Message}");
            return null!;
        }
        
    }

    public async Task<ProductEntity> GetProductByArticleAsync(string articleNumber)
    {
        try
        {
            return await _productRepository.GetOneAsync(p => p.ArticleNumber == articleNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting product: {ex.Message}");
            Debug.WriteLine($"Error getting product: {ex.Message}");
            return null!;
        }
    }

    public async Task<IEnumerable<ProductEntity>> GetAllProductAsync()
    {
        try
        {
            return await _productRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return [];
        }
    }

    public async Task<ProductEntity> UpdateProductAsync(ProductEntity product)
    {
        try
        {            
            return await _productRepository.UpdateAsync(p => p.ArticleNumber == product.ArticleNumber, product);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in updating product: {ex.Message}");
            Debug.WriteLine($"Error in updating product: {ex.Message}");
            return null!;
        }
    }

    public async Task<bool> DeleteProductByArticleAsync(string articleNumber)
    {

        try
        {
            await _productRepository.RemoveAsync(p => p.ArticleNumber == articleNumber);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in deleting product: {ex.Message}");
            Debug.WriteLine($"Error in deleting product: {ex.Message}");
            return false;
        }
    }

    
}
