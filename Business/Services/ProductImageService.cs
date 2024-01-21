using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Drawing;

namespace Business.Services
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
        
        public async Task<bool> AddProductImage(ProductImage productImage)
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
    }
}
