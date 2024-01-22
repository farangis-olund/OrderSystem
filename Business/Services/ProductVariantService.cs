using Shared.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Business.Services;

public class ProductVariantService
{
    private readonly ProductVariantRepository _productVariantRepository;
    
    private readonly SizeRepository _sizeRepository;
    private readonly ColorRepository _colorRepository;
    
    private readonly ILogger<ProductVariantService> _logger;
    public ProductVariantService ( ProductVariantRepository productVariantRepository,
                                   SizeRepository sizeRepository,
                                   ColorRepository colorRepository,
                                   ILogger<ProductVariantService> logger)
    {
        _productVariantRepository = productVariantRepository;
        _sizeRepository = sizeRepository;
        _colorRepository = colorRepository;
        _logger = logger;
    }

    public async Task<ProductVariantEntity?> AddProductVariant(ProductVariant productVariant)
    {
        try
        {
            if (await _productVariantRepository.Exist(pv =>
                pv.ArticleNumber == productVariant.ArticleNumber &&
                pv.Color.ColorName == productVariant.ColorName &&
                pv.SizeId == productVariant.SizeId))
            {
                return null;
            }

            var size = await GetOrCreateSize(productVariant.Size);
            var color = await GetOrCreateColor(productVariant.ColorName);

            var newProductVariant = new ProductVariantEntity
            {
                ArticleNumber = productVariant.ArticleNumber,
                Quantity = productVariant.Quantity,
                Size = size!,
                Color = color!
            };

            return await _productVariantRepository.AddAsync(newProductVariant);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding product: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<ProductVariantEntity?> GetProductVariant(ProductVariant productVariant)
    {
        try
        {
            var (articleNumber, colorId, sizeId) = (productVariant.ArticleNumber, productVariant.ColorId, productVariant.SizeId);

            if (string.IsNullOrEmpty(articleNumber) || colorId==0 || sizeId == 0)
            {
                _logger.LogError("Invalid product variant parameters.");
                return null;
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
            return null;
        }
    }

    public async Task<ProductVariantEntity?> GetProductVariantById(int id)
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
            return null;
        }
    }

    public async Task<IEnumerable<ProductVariantEntity>> GetAllProductVariants()
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


    private async Task<SizeEntity?> GetOrCreateSize(ProductSize size)
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
            return null;
        }
    }

    private async Task<ColorEntity?> GetOrCreateColor(string colorName)
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
            return null;
        }
    }


}
