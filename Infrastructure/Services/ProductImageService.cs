using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class ProductImageService
    {
        private readonly ProductImageRepository _productImageRepository;
        private readonly ImageRepository _imageRepository;
        private readonly ILogger<ProductImageService> _logger;

        public ProductImageService(ProductImageRepository productImageRepository, ImageRepository imageRepository, ILogger<ProductImageService> logger)
        {
            _productImageRepository = productImageRepository;
            _imageRepository = imageRepository;
            _logger = logger;
        }
        
        public async Task<bool> AddProductImageAsync(ProductImage productImage)
        {

            try
            {
                var newImage = await _imageRepository.GetOneAsync(i => i.ImageUrl == productImage.ImageUrl);
                newImage ??= await _imageRepository.AddAsync(new ImageEntity { ImageUrl = productImage.ImageUrl });

                var existingImage = await _productImageRepository.Exist(
                                    pi => pi.ProductVariantId == productImage.ProductVariantId && 
                                    pi.ArticleNumber == productImage.ArticleNumber && 
                                    pi.ImageId == newImage.Id);

                if (!existingImage)
                {
                    var newProductImage = new ProductImageEntity
                    {
                        ProductVariantId = productImage.ProductVariantId,
                        ArticleNumber = productImage.ArticleNumber,
                        Image = newImage
                    };
                    await _productImageRepository.AddAsync(newProductImage);
                }
                    
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding product image: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<ProductImageEntity> GetProductImageAsync(int productVariantId, int imageId)
        {

            try
            {
                var existingProductImage = await _productImageRepository.GetOneAsync(
                                    pi => pi.ProductVariantId == productVariantId &&
                                    pi.ImageId == imageId);

                if (existingProductImage !=null)
                {
                    return existingProductImage;
                }
                else
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting product image: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ProductImageEntity> GetProductImageAsync(int productVariantId)
        {

            try
            {
                var existingProductImage = await _productImageRepository.GetOneAsync(pi => pi.ProductVariantId == productVariantId);

                if (existingProductImage != null)
                {
                    return existingProductImage;
                }
                else
                    return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting product image: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ProductImageEntity> UpdateProductImageAsync(int productVariantId, string articleNumber, string imageUrl)
        {

            try
            {
                var existingImage = await _imageRepository.GetOneAsync(i => i.ImageUrl == imageUrl);
                existingImage ??= await _imageRepository.AddAsync(new ImageEntity { ImageUrl = imageUrl });

                var existingProductImage = await _productImageRepository.GetOneAsync(
                                    pi => pi.ProductVariantId == productVariantId &&
                                    pi.ArticleNumber == articleNumber);

                if (existingProductImage != null)
                {
                    existingProductImage.ImageId = existingImage.Id;
                    Func<ProductImageEntity, object> keySelector = p => p.Id;
                    return await _productImageRepository.UpdateAsync(existingProductImage, keySelector);
                }
                else
                    return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in updating product image: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<bool> DeleteProductImageAsync(ProductImage productImage)
        {

            try
            {
                var existingProductImage = await _productImageRepository.Exist(
                                    pi => pi.ProductVariantId == productImage.ProductVariantId &&
                                    pi.ArticleNumber == productImage.ArticleNumber && pi.ImageId == productImage.ImageId);

                if (existingProductImage)
                {
                    return await _productImageRepository.RemoveAsync(productImage);
                }
                else
                    return false;
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
