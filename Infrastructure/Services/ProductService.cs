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
    private readonly ProductVariantService _productVariantService;
    private readonly ProductImageService _productImageService;
    private readonly ImageRepository _imageRepository;
    private readonly ProductPriceService _productPriceService;
    private readonly ColorRepository _colorRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(BrandRepository brandRepository, 
                          CategoryRepository categoryRepository, 
                          ProductRepository productRepository,
                          ProductImageService productImageService,
                          ProductVariantService productVariantService,
                          ImageRepository imageRepository,
                          ProductPriceService productPriceService,
                          ColorRepository colorRepository,
                          ILogger<ProductService> logger)
    {
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _productImageService = productImageService;
        _productVariantService = productVariantService;
        _imageRepository = imageRepository;
        _productPriceService = productPriceService;
        _colorRepository = colorRepository;
        _logger = logger;
    }

    public async Task<bool> AddProductAsync(Product product)
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
                BrandId = existingBrand.Id,
                CategoryId = existingCategory.Id
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

    public async Task<ProductEntity> GetProductByArticleAsync(string articleNumber)
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

    public async Task<IEnumerable<ProductEntity>> GetAllProductAsync()
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

    public async Task<Product> UpdateProductAsync(Product product)
    {

        try
        {
            var existingProduct = await _productRepository.GetOneAsync(p => p.ArticleNumber == product.ArticleNumber);

            if (existingProduct != null)
            {
                object keySelector(ProductEntity p) => p.ArticleNumber;
                return await _productRepository.UpdateAsync(product, keySelector);
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

    public async Task<bool> DeleteProductByArticleAsync(string articleNumber)
    {

        try
        {
            var existingProduct = await _productRepository.GetOneAsync(p => p.ArticleNumber == articleNumber);
            if (existingProduct != null)
            {
                await _productVariantService.DeleteProductVariantByArticleAsync(articleNumber);
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
