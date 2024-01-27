using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class PriceService
    {
        private readonly PriceRepository _productPriceRepository;
        private readonly CurrencyService _currencyService;
        private readonly ILogger<ProductImageService> _logger;

        public PriceService( PriceRepository productPriceRepository,
                                    CurrencyService currencyService,
                                    ILogger<ProductImageService> logger)
        {
            _productPriceRepository = productPriceRepository;
            _currencyService = currencyService;
            _logger = logger;
        }


        public async Task<ProductPriceEntity> AddPriceAsync(ProductVariantEntity productVariant, ProductDetail productDetail)
        {
            try
            {
              var currency = await _currencyService.AddCurrencyAsync(productDetail.CurrencyCode, productDetail.CurrencyName);

                var existingPrice = await _productPriceRepository.ExistsAsync(
                            pp => pp.ProductVariantId == productVariant.Id &&
                            pp.ArticleNumber == productVariant.ArticleNumber &&
                            pp.Price == productDetail.Price);

                if (existingPrice)
                {
                    return null!;
                }
                return await _productPriceRepository.AddAsync(new ProductPriceEntity
                {
                    ProductVariantId = productVariant.Id,
                    ArticleNumber = productVariant.ArticleNumber,
                    Price = productDetail.Price,
                    DiscountPrice = productDetail.DiscountPrice,
                    DicountPercentage = productDetail.DicountPercentage,
                    CurrencyCode = currency.Code
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding product price: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            } 
        }

        public async Task<ProductPriceEntity> GetPriceAsync(int id)
        {
            try
            {
                return await _productPriceRepository.GetOneAsync(pp => pp.Id ==id);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting product price: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ProductPriceEntity> GetPriceByProductVariantAsync(int productVariantId, string articleNumber)
        {
            try
            {
                return await _productPriceRepository.GetOneAsync(pp => pp.ProductVariantId == productVariantId &&
                            pp.ArticleNumber == articleNumber);
                
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
                    return await _productPriceRepository.UpdateAsync(p => p.Id == existingPrice.Id, existingPrice);
                    
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
                 await _productPriceRepository.RemoveAsync(pp => pp.ProductVariantId == productVariantId &&
                        pp.ArticleNumber == articleNumber);
                 return true;
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
