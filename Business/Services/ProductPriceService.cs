using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Business.Services
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


        public async Task<bool> AddProductPrice(ProductPrice productPrice)
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
                     await CreatePrice(new ProductPriceEntity
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

        private async Task<ProductPriceEntity> CreatePrice(ProductPriceEntity newPrice)
        {
            try
            {
                await _productPriceRepository.AddAsync(newPrice);
                return newPrice;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product price: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

    }
}
