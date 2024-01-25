using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ProductVariantService
{
    private readonly ProductVariantRepository _productVariantRepository;
    
    private readonly SizeRepository _sizeRepository;
    private readonly ColorRepository _colorRepository;
    private readonly ProductImageRepository _productImageRepository;
    private readonly ProductPriceRepository _productPriceRepository;
    private readonly ILogger<ProductVariantService> _logger;
    public ProductVariantService ( ProductVariantRepository productVariantRepository,
                                   SizeRepository sizeRepository,
                                   ColorRepository colorRepository,
                                   ProductImageRepository productImageRepository,
                                   ProductPriceRepository productPriceRepository,
                                   ILogger<ProductVariantService> logger)
    {
        _productVariantRepository = productVariantRepository;
        _sizeRepository = sizeRepository;
        _colorRepository = colorRepository;
        _productImageRepository = productImageRepository;
        _productPriceRepository = productPriceRepository;
        _logger = logger;
    }

    public async Task<ProductVariantEntity> AddProductVariantAsync(ProductVariant productVariant)
    {
        try
        {
            if (await _productVariantRepository.Exist(pv =>
                pv.ArticleNumber == productVariant.ArticleNumber &&
                pv.Color.ColorName == productVariant.ColorName &&
                pv.SizeId == productVariant.SizeId))
            {
                return null!;
            }
          
            var color = await GetOrCreateColorAsync(productVariant.ColorName);

            productVariant.ColorId = color.Id!;
            

            return await _productVariantRepository.AddAsync(productVariant);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding product: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<ProductVariantEntity> GetProductVariantAsync(ProductVariant productVariant)
    {
        try
        {
            var (articleNumber, colorId, sizeId) = (productVariant.ArticleNumber, productVariant.ColorId, productVariant.SizeId);

            if (string.IsNullOrEmpty(articleNumber) || colorId==0 || sizeId == 0)
            {
                _logger.LogError("Invalid product variant parameters.");
                return null!;
            }
                
            var existingProductVariant = await _productVariantRepository.GetOneAsync(pv =>
                pv.ArticleNumber == articleNumber &&
                pv.ColorId == colorId &&
                pv.SizeId == sizeId);

            return existingProductVariant;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<ProductVariantEntity> GetProductVariantByArticleNumberAsync(string articleNumber)
    {
        try
        {
            var existingProductVariant = await _productVariantRepository.GetOneAsync(pv => pv.ArticleNumber == articleNumber); 

            return existingProductVariant;
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
            var existingProductVariant = await _productVariantRepository.GetOneAsync(pv => pv.Id == id);

            return existingProductVariant;
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
            var existingProductVariant = await _productVariantRepository.GetAllAsync();

            return existingProductVariant;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return [];
        }
    }

    public async Task<ProductVariantEntity> UpdateProductVariantAsync(ProductVariant productVariant)
    {
        try
        {
            Func<ProductVariantEntity, object> keySelector = p => p.Id;
            var result = await _productVariantRepository.UpdateAsync(productVariant, keySelector);
            if (result != null)
            {
                return result;
            }
            else
            return null!;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<bool> DeleteProductVariantByArticleAsync(string productArticle)
    {
        try
        {
            var existingProductVariant = await _productVariantRepository.GetOneAsync(p => p.ArticleNumber == productArticle);

            if (existingProductVariant != null)
            {
                await _productImageRepository.RemoveAsync(pi => pi.ArticleNumber == productArticle &&
                                                          pi.ProductVariantId == existingProductVariant.Id);
                await _productPriceRepository.RemoveAsync(pi => pi.ArticleNumber == productArticle &&
                                                          pi.ProductVariantId == existingProductVariant.Id);
                await _productVariantRepository.RemoveAsync(pv => pv.ArticleNumber == productArticle);
                return true;
            }
            else
                return false;
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving product variant: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    private async Task<SizeEntity> GetOrCreateSizeAsync(ProductSize size)
    {
        try
        {
            var existingSize = await _sizeRepository.GetOneAsync(s =>
                s.SizeType == size.SizeType &&
                s.SizeValue == size.SizeValue &&
                s.AgeGroup == size.AgeGroup
            );
            
            if (existingSize != null)
                return existingSize;
                        
            return await _sizeRepository.AddAsync(new SizeEntity
            {
                SizeType = size.SizeType,
                SizeValue = size.SizeValue,
                AgeGroup = size.AgeGroup
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting or creating size: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    private async Task<ColorEntity> GetOrCreateColorAsync(string colorName)
    {
        try
        {
            var existingColor = await _colorRepository.GetOneAsync(c => c.ColorName == colorName);

            if (existingColor != null)
                return existingColor;

            return await _colorRepository.AddAsync(new ColorEntity { ColorName = colorName });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting or creating color: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }


}
