using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class ProductPriceService
    {
        private readonly ProductPriceRepository _productPriceRepository;
        private readonly CurrencyRepository _currencyRepository;
        private readonly ILogger<ProductImageService> _logger;

        public ProductPriceService( ProductPriceRepository productPriceRepository,
                                    CurrencyRepository currencyRepository,
                                    ILogger<ProductImageService> logger)
        {
            _productPriceRepository = productPriceRepository;
            _currencyRepository = currencyRepository;
            _logger = logger;
        }


        public async Task<bool> AddProductPriceAsync(ProductPrice productPrice)
        {
            try
            {
                var newCurrency = await _currencyRepository.GetOneAsync(c => c.Code == productPrice.Code);
                newCurrency ??= await _currencyRepository.AddAsync(new CurrencyEntity
                {
                    Code = productPrice.Code,
                    CurrencyName = productPrice.CurrencyName
                });

                var existingPrice = await _productPriceRepository.Exist(
                            pp => pp.ProductVariantId == productPrice.ProductVariantId &&
                            pp.ArticleNumber == productPrice.ArticleNumber &&
                            pp.Price == productPrice.Price);

                if (!existingPrice)
                {
                    await _productPriceRepository.AddAsync(new ProductPriceEntity
                    {
                        ProductVariantId = productPrice.ProductVariantId,
                        ArticleNumber = productPrice.ArticleNumber,
                        Price = productPrice.Price,
                        DiscountPrice = productPrice.DiscountPrice,
                        DicountPercentage = productPrice.DicountPercentage,
                        CurrencyCode = newCurrency.Code
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding product price: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return false;
            } 
        }

        public async Task<ProductPriceEntity> GetProductPriceAsync(int id)
        {
            try
            {
                var existingPrice = await _productPriceRepository.GetOneAsync(pp => pp.Id ==id);

                if (existingPrice != null)
                {
                    return existingPrice;
                }
                else
                    return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting product price: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ProductPriceEntity> GetProductPriceByProductVariantAsync(int productVariantId, string articleNumber)
        {
            try
            {
                var existingPrice = await _productPriceRepository.GetOneAsync(pp => pp.ProductVariantId == productVariantId &&
                            pp.ArticleNumber == articleNumber);

                if (existingPrice != null)
                {
                    return existingPrice;
                }
                else
                    return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting product price by Product variant: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ProductPriceEntity> UpdateProductPriceByProductVariantAsync(int productVariantId, string articleNumber, decimal price)
        {
            try
            {
                var existingPrice = await _productPriceRepository.GetOneAsync(pp => pp.ProductVariantId == productVariantId &&
                            pp.ArticleNumber == articleNumber);

                if (existingPrice != null)
                {
                    existingPrice.Price = price;
                    Func<ProductPriceEntity, object> keySelector = p => p.Id;
                    return await _productPriceRepository.UpdateAsync(existingPrice, keySelector);
                    
                }
                else
                    return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in updating product price by Product variant: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<bool> DeleteProductPriceByProductVariantAsync(int productVariantId, string articleNumber)
        {
            try
            {
                var existingPrice = await _productPriceRepository.GetOneAsync(pp => pp.ProductVariantId == productVariantId &&
                            pp.ArticleNumber == articleNumber);

                if (existingPrice != null)
                {
                    await _productPriceRepository.RemoveAsync(existingPrice);
                    return true;
                }
                else
                    return false!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in deleting product price by Product variant: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
