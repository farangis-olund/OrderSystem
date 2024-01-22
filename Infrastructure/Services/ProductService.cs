using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services;

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

    public async Task<bool> AddProduct(Product product)
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

    public async Task<ProductEntity> GetProductByArticle(string articleNumber)
    {

        try
        {
            var existingProduct = await _productRepository.GetOneAsync(p => p.ArticleNumber == articleNumber);

            if (existingProduct != null)
            {
                return existingProduct;
            }
            else
                return null!;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in geting product: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<IEnumerable<ProductEntity>> GetAllProduct()
    {
        try
        {
            var existingProduct = await _productRepository.GetAllAsync();

            return existingProduct;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return [];
        }
    }

    public async Task<Product> UpdateProduct(Product product)
    {

        try
        {
            var existingProduct = await _productRepository.GetOneAsync(p => p.ArticleNumber == product.ArticleNumber);

            if (existingProduct != null)
            {
                return await _productRepository.UpdateAsync(product);
            }
            else
                return null!;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in updating product: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<bool> DeleteProductByArticle(string articleNumber)
    {

        try
        {
            var existingProduct = await _productRepository.GetOneAsync(p => p.ArticleNumber == articleNumber);

            if (existingProduct != null)
            {
                await _productRepository.RemoveAsync(existingProduct);
                return true;
            }
            else
                return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in deleting product: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
}
