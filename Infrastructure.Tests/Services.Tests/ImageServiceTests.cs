using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;


namespace Infrastructure.Tests.Services.Tests
{
    public class ImageServiceTests
    {
        private readonly InMemoryImageRepository _inMemoryImageRepository;
        private readonly Mock<ILogger<ImageService>> _mockLogger;

        public ImageServiceTests()
        {
            var context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _inMemoryImageRepository = new InMemoryImageRepository(context);
            _mockLogger = new Mock<ILogger<ImageService>>();
        }

        [Fact]
        public async Task AddImageAsync_ShouldAddImage_WhenImageDoesNotExist()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockLogger.Object);
            var imageUrl = "NewImageUrl";

            // Act
            var result = await imageService.AddImageAsync(imageUrl);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(imageUrl, result.ImageUrl);
        }

        [Fact]
        public async Task AddImageAsync_ShouldReturnExistingImage_WhenImageExists()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockLogger.Object);
            var imageUrl = "ExistingImageUrl";
            var existingImage = new ImageEntity { ImageUrl = imageUrl };
            await _inMemoryImageRepository.AddAsync(existingImage);

            // Act
            var result = await imageService.AddImageAsync(imageUrl);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingImage, result);
        }

        [Fact]
        public async Task GetImageAsync_ShouldReturnImage_WhenImageExists()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockLogger.Object);
            var imageUrl = "ExistingImageUrl";
            var existingImage = new ImageEntity { ImageUrl = imageUrl };
            await _inMemoryImageRepository.AddAsync(existingImage);

            // Act
            var result = await imageService.GetImageAsync(imageUrl);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingImage, result);
        }

        [Fact]
        public async Task GetImageAsync_ShouldReturnNull_WhenImageDoesNotExist()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockLogger.Object);
            var imageUrl = "NonExistentImageUrl";

            // Act
            var result = await imageService.GetImageAsync(imageUrl);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateImageAsync_ShouldUpdateImage_WhenImageExists()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockLogger.Object);
            var imageUrl = "ExistingImageUrl";
            var existingImage = new ImageEntity { ImageUrl = imageUrl };
            await _inMemoryImageRepository.AddAsync(existingImage);

            // Act
            existingImage.ImageUrl = "UpdatedImageUrl";
            var result = await imageService.UpdateimageAsync(existingImage);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedImageUrl", result.ImageUrl);
        }

        [Fact]
        public async Task GetAllImagesAsync_ShouldReturnAllImages_WhenImagesExist()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockLogger.Object);
            var testData = new List<ImageEntity>
            {
                new() { ImageUrl = "Test1" },
                new() { ImageUrl = "Test2" }
            };

            foreach (var image in testData)
            {
                await _inMemoryImageRepository.AddAsync(image);
            }

            // Act
            var result = await imageService.GetAllimagesAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(testData.Count, result.Count());
        }

        [Fact]
        public async Task DeleteImageAsync_ShouldDeleteImage_WhenImageExists()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockLogger.Object);
            var imageUrl = "ImageToDelete";
            var imageToDelete = new ImageEntity { ImageUrl = imageUrl };
            await _inMemoryImageRepository.AddAsync(imageToDelete);

            // Act
            var result = await imageService.DeleteImageAsync(imageUrl);

            // Assert
            Assert.True(result);

            var deletedImage = await _inMemoryImageRepository.GetOneAsync(b => b.ImageUrl == imageUrl);
            Assert.Null(deletedImage);
        }

        [Fact]
        public async Task DeleteImageAsync_ShouldNotDeleteImage_WhenImageDoesNotExist()
        {
            // Arrange
            var imageService = new ImageService(_inMemoryImageRepository, _mockLogger.Object);
            var imageUrl = "NonExistentImageUrl";

            // Act
            var result = await imageService.DeleteImageAsync(imageUrl);

            // Assert
            Assert.False(result);
        }
    }

    public class InMemoryImageRepository : ImageRepository
    {
        private readonly List<ImageEntity> _images;

        public InMemoryImageRepository(ProductDataContext context) : base(context)
        {
            _images = new List<ImageEntity>();
        }

        public override Task<IEnumerable<ImageEntity>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<ImageEntity>>(_images);
        }

        public override Task<ImageEntity> GetOneAsync(Expression<Func<ImageEntity, bool>> predicate)
        {
            return Task.FromResult(_images.AsQueryable().FirstOrDefault(predicate.Compile())!);
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
            }
            return Task.FromResult(existingImage!);
        }

        public override Task<bool> RemoveAsync(Expression<Func<ImageEntity, bool>> predicate)
        {
            var entityToRemove = _images.FirstOrDefault(predicate.Compile());
            if (entityToRemove != null)
            {
                _images.Remove(entityToRemove);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
