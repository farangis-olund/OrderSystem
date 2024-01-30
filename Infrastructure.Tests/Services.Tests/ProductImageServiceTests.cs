using System.Linq.Expressions;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Infrastructure.Tests.Services.Tests
{
    public class ProductImageServiceTests
    {
        private readonly InMemoryProductImageRepository _inMemoryProductImageRepository;
        private readonly Mock<ILogger<ProductImageService>> _mockLogger;
        private readonly Mock<ILogger<ImageService>> _mockImageService;
        private readonly InMemoryImageRepository _inMemoryImageRepository;
        public ProductImageServiceTests()
        {
            var context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
               .UseInMemoryDatabase($"{Guid.NewGuid()}")
               .Options);
            
            _inMemoryProductImageRepository = new InMemoryProductImageRepository(context);
            _inMemoryImageRepository = new InMemoryImageRepository(context);
            _mockImageService = new Mock<ILogger<ImageService>>();

            _mockLogger = new Mock<ILogger<ProductImageService>>();
        }

        [Fact]
        public async Task AddProductImageAsync_ShouldAddProductImage_WhenImageDoesNotExist()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockImageService.Object); 
            var productImageService = new ProductImageService(_inMemoryProductImageRepository, imageService, _mockLogger.Object);
            var productVariant = new ProductVariantEntity { Id = 1, ArticleNumber = "123" };
            var imageUrl = "https://example.com/image.jpg";

            // Act
            var result = await productImageService.AddProductImageAsync(productVariant, imageUrl);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddProductImageAsync_ShouldNotAddProductImage_WhenImageExists()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockImageService.Object);
            var productImageService = new ProductImageService(_inMemoryProductImageRepository, imageService, _mockLogger.Object);
            var productVariant = new ProductVariantEntity { Id = 1, ArticleNumber = "123" };
            var imageUrl = "https://example.com/image.jpg";
            await _inMemoryProductImageRepository.AddAsync(new ProductImageEntity { ProductVariantId = productVariant.Id, ArticleNumber = productVariant.ArticleNumber, ImageId = 1 });

            // Act
            var result = await productImageService.AddProductImageAsync(productVariant, imageUrl);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductImageAsync_ShouldReturnProductImage_WhenProductImageExists()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockImageService.Object);
            var productImageService = new ProductImageService(_inMemoryProductImageRepository, imageService, _mockLogger.Object);
            var productVariantId = 1;
            var imageId = 1;
            await _inMemoryProductImageRepository.AddAsync(new ProductImageEntity { ProductVariantId = productVariantId, ImageId = imageId });

            // Act
            var result = await productImageService.GetProductImageAsync(productVariantId, imageId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productVariantId, result.ProductVariantId);
            Assert.Equal(imageId, result.ImageId);
        }

        [Fact]
        public async Task GetProductImageAsync_ShouldReturnNull_WhenProductImageDoesNotExist()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockImageService.Object);
            var productImageService = new ProductImageService(_inMemoryProductImageRepository, imageService, _mockLogger.Object);

            // Act
            var result = await productImageService.GetProductImageAsync(1, 1);

            // Assert
            Assert.Null(result);
        }

       
        [Fact]
        public async Task DeleteProductImageAsync_ShouldDeleteProductImage_WhenProductImageExists()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockImageService.Object);
            var productImageService = new ProductImageService(_inMemoryProductImageRepository, imageService, _mockLogger.Object);
            var productVariantId = 1;
            var articleNumber = "123";
            var imageUrl = "https://example.com/image.jpg";
            await _inMemoryProductImageRepository.AddAsync(new ProductImageEntity { ProductVariantId = productVariantId, ArticleNumber = articleNumber, Image = new ImageEntity { Id = 1, ImageUrl = imageUrl } });

            // Act
            var result = await productImageService.DeleteProductImageAsync(productVariantId, articleNumber, imageUrl);

            // Assert
            Assert.True(result);

            var deletedProductImage = await _inMemoryProductImageRepository.GetOneAsync(pi => pi.ProductVariantId == productVariantId && pi.ArticleNumber == articleNumber && pi.Image.ImageUrl == imageUrl);
            Assert.Null(deletedProductImage);
        }

        public class InMemoryProductImageRepository : ProductImageRepository
        {
            private readonly List<ProductImageEntity> _productImages;

            public InMemoryProductImageRepository(ProductDataContext context) : base(context)
            {
                _productImages = new List<ProductImageEntity>();
            }

            public override Task<ProductImageEntity> GetOneAsync(Expression<Func<ProductImageEntity, bool>> predicate)
            {
                return Task.FromResult(_productImages.AsQueryable().FirstOrDefault(predicate.Compile())!);
            }

            public override Task<ProductImageEntity> AddAsync(ProductImageEntity entity)
            {
                _productImages.Add(entity);
                return Task.FromResult(entity);
            }

            public override Task<ProductImageEntity> UpdateAsync(Expression<Func<ProductImageEntity, bool>> predicate, ProductImageEntity entity)
            {
                var existingProductImage = _productImages.AsQueryable().FirstOrDefault(predicate.Compile());
                if (existingProductImage != null)
                {
                    existingProductImage.ProductVariantId = entity.ProductVariantId;
                    existingProductImage.ArticleNumber = entity.ArticleNumber;
                    existingProductImage.ImageId = entity.ImageId;

                    return Task.FromResult(existingProductImage);
                }

                return Task.FromResult<ProductImageEntity>(null!);
            }

            public override Task<bool> RemoveAsync(Expression<Func<ProductImageEntity, bool>> predicate)
            {
                var productImageToRemove = _productImages.AsQueryable().FirstOrDefault(predicate.Compile());
                if (productImageToRemove != null)
                {
                    _productImages.Remove(productImageToRemove);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
        }

        public class InMemoryImageRepository : ImageRepository
        {
            private readonly List<ImageEntity> _images;

            public InMemoryImageRepository(ProductDataContext context) : base(context)
            {
                _images = new List<ImageEntity>();
            }

            public override Task<ImageEntity> GetOneAsync(Expression<Func<ImageEntity, bool>> predicate)
            {
                return Task.FromResult(_images.AsQueryable().FirstOrDefault(predicate.Compile())!);
            }

            public override Task<IEnumerable<ImageEntity>> GetAllAsync()
            {
                return Task.FromResult<IEnumerable<ImageEntity>>(_images);
            }

            public override Task<ImageEntity> AddAsync(ImageEntity entity)
            {
                _images.Add(entity);
                return Task.FromResult(entity);
            }

            public override Task<ImageEntity> UpdateAsync(Expression<Func<ImageEntity, bool>> predicate, ImageEntity entity)
            {
                var existingImage = _images.AsQueryable().FirstOrDefault(predicate.Compile());
                if (existingImage != null)
                {
                    existingImage.ImageUrl = entity.ImageUrl; 
                    return Task.FromResult(existingImage);
                }

                return Task.FromResult<ImageEntity>(null!);
            }

            public override Task<bool> RemoveAsync(Expression<Func<ImageEntity, bool>> predicate)
            {
                var imageToRemove = _images.AsQueryable().FirstOrDefault(predicate.Compile());
                if (imageToRemove != null)
                {
                    _images.Remove(imageToRemove);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
        }

    }
}
