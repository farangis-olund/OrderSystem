using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Services
{
    public class ProductImageService
    {
        private readonly ProductImageRepository _productImageRepository;
        private readonly ImageService _imageService;
        private readonly ILogger<ProductImageService> _logger;

        public ProductImageService(ProductImageRepository productImageRepository, ImageService imageService, ILogger<ProductImageService> logger)
        {
            _productImageRepository = productImageRepository;
            _imageService = imageService;
            _logger = logger;
        }
        
        public async Task<ProductImageEntity> AddProductImageAsync(ProductVariantEntity productVariant, string imageUrl)
        {

            try
            {
                var imageEntity = await _imageService.AddImageAsync(imageUrl);
               
                var existingImage = await _productImageRepository.ExistsAsync(
                                    pi => pi.ProductVariantId == productVariant.Id && 
                                    pi.ArticleNumber == productVariant.ArticleNumber && 
                                    pi.ImageId == imageEntity.Id);

                if (!existingImage)
                {
                    await _productImageRepository.AddAsync(new ProductImageEntity
                    {
                        ProductVariantId = productVariant.Id,
                        ArticleNumber = productVariant.ArticleNumber,
                        ImageId = imageEntity.Id
                    });
                    
                }
                    
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding product image: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ProductImageEntity> GetProductImageAsync(int productVariantId, int imageId)
        {

            try
            {
                return await _productImageRepository.GetOneAsync(pi => pi.ProductVariantId == productVariantId && pi.ImageId == imageId);
   
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting product image: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ProductImageEntity> GetProductImageByProductVariantIdAsync(int productVariantId)
        {

            try
            {
                return await _productImageRepository.GetOneAsync(pi => pi.ProductVariantId == productVariantId);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting product image: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ProductImageEntity> UpdateProductImageAsync(ProductVariantEntity productVariant, string imageUrl)
        {

            try
            {
                var imageEntity = await _imageService.AddImageAsync(imageUrl);
                var productImageEntity = await GetProductImageByProductVariantIdAsync(productVariant.Id);
                productImageEntity.ImageId = imageEntity.Id;
                return await _productImageRepository.UpdateAsync(pi => pi.ProductVariantId == productVariant.Id && 
                                                            pi.ArticleNumber == productVariant.ArticleNumber, productImageEntity);
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in updating product image: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<bool> DeleteProductImageAsync(int productVariantId, string articleNumber, string imageUrl)
        {

            try
            {
                return await _productImageRepository.RemoveAsync(pi => pi.ProductVariantId == productVariantId &&
                              pi.ArticleNumber == articleNumber && pi.Image.ImageUrl == imageUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in updating product image: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
