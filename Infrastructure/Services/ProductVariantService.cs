using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ProductVariantService
{
    private readonly ProductVariantRepository _productVariantRepository;
    private readonly ProductService _productService;
    private readonly SizeService _sizeService;
    private readonly ColorService _colorService;
    private readonly ILogger<ProductVariantService> _logger;
    public ProductVariantService ( ProductVariantRepository productVariantRepository,
                                   ProductService productService,
                                   SizeService sizeService,
                                   ColorService colorService,
                                   ILogger<ProductVariantService> logger)
    {
        _productVariantRepository = productVariantRepository;
        _productService = productService;
        _sizeService = sizeService;
        _colorService = colorService;
        _logger = logger;
    }

    public async Task<ProductVariantEntity> AddProductVariantAsync(ProductDetail productVariant)
    {
        try
        {

            var product = await _productService.GetProductByArticleAsync(productVariant.ArticleNumber);
            product ??= await _productService.AddProductAsync(
                    new Product
                    {
                        ArticleNumber = productVariant.ArticleNumber,
                        ProductName = productVariant.ProductName,
                        ProductInfo = productVariant.ProductInfo,
                        Material = productVariant.Material,
                        CategoryName = productVariant.CategoryName,
                        BrandName = productVariant.BrandName
                    });
            
            if (await _productVariantRepository.GetOneAsync(pv =>
                pv.ArticleNumber == product.ArticleNumber &&
                pv.Color.ColorName == productVariant.ColorName &&
                pv.Size.Id == productVariant.SizeId) != null)

            {
                return null!;
            }
          
            var color = await _colorService.AddColorAsync(productVariant.ColorName);

            return await _productVariantRepository.AddAsync(new ProductVariantEntity
            {
                ArticleNumber = productVariant.ArticleNumber,
                Quantity = productVariant.Quantity,
                ColorId = color.Id,
                SizeId = productVariant.SizeId

            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<ProductVariantEntity> GetProductVariantAsync(ProductVariantEntity productVariant)
    {
        try
        {
            var (articleNumber, colorName) = (productVariant.ArticleNumber, productVariant.Color.ColorName);

            if (string.IsNullOrEmpty(articleNumber) || string.IsNullOrEmpty(colorName))
            {
                _logger.LogError("Invalid product variant parameters.");
                return null!;
            }
                
            return await _productVariantRepository.GetOneAsync(pv => pv.ArticleNumber == articleNumber &&  
                                                                                     pv.Color.ColorName == colorName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<ProductVariantEntity> GetProductVariantByIdAsync(int id)
    {
        try
        {
            return await _productVariantRepository.GetOneAsync(pv => pv.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<IEnumerable<ProductVariantEntity>> GetAllProductVariantsAsync()
    {
        try
        {
           return await _productVariantRepository.GetAllAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return [];
        }
    }

    public async Task<ProductVariant> UpdateProductVariantAsync(ProductVariantEntity productVariant)
    {
        try
        {
            return await _productVariantRepository.UpdateAsync(pv => pv.Id == productVariant.Id, productVariant);
          
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<bool> DeleteProductVariantByArticleAsync(ProductVariantEntity productVariant)
    {
        try
        {
            return await _productVariantRepository.RemoveAsync(p => p.ArticleNumber == productVariant.ArticleNumber && p.Id == productVariant.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

}
