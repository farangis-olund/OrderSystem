using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;


namespace Infrastructure.Tests.Services.Tests
{
    public class SizeServiceTests
    {
        private readonly InMemorySizeRepository _inMemorySizeRepository;
        private readonly Mock<ILogger<SizeService>> _mockLogger;

        public SizeServiceTests()
        {
            var context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _inMemorySizeRepository = new InMemorySizeRepository(context);
            _mockLogger = new Mock<ILogger<SizeService>>();
        }

        [Fact]
        public async Task AddSizeAsync_ShouldAddSize_WhenSizeDoesNotExist()
        {
            // Arrange
            var sizeService = new SizeService(_inMemorySizeRepository, _mockLogger.Object);
            var productSize = new ProductSize { SizeType = "Small", SizeValue = "S", AgeGroup = "Adult" };

            // Act
            var result = await sizeService.AddSizeAsync(productSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productSize.SizeType, result.SizeType);
            Assert.Equal(productSize.SizeValue, result.SizeValue);
            Assert.Equal(productSize.AgeGroup, result.AgeGroup);
        }

        [Fact]
        public async Task AddSizeAsync_ShouldReturnExistingSize_WhenSizeExists()
        {
            // Arrange
            var sizeService = new SizeService(_inMemorySizeRepository, _mockLogger.Object);
            var productSize = new ProductSize { SizeType = "Small", SizeValue = "S", AgeGroup = "Adult" };
            var existingSize = new SizeEntity { SizeType = productSize.SizeType, SizeValue = productSize.SizeValue, AgeGroup = productSize.AgeGroup };
            await _inMemorySizeRepository.AddAsync(existingSize);

            // Act
            var result = await sizeService.AddSizeAsync(productSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingSize, result);
        }

        [Fact]
        public async Task GetSizeAsync_ShouldReturnSize_WhenSizeExists()
        {
            // Arrange
            var sizeService = new SizeService(_inMemorySizeRepository, _mockLogger.Object);
            var productSize = new ProductSize { SizeType = "Small", SizeValue = "S", AgeGroup = "Adult" };
            var existingSize = new SizeEntity { SizeType = productSize.SizeType, SizeValue = productSize.SizeValue, AgeGroup = productSize.AgeGroup };
            await _inMemorySizeRepository.AddAsync(existingSize);

            // Act
            var result = await sizeService.GetSizeAsync(productSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingSize, result);
        }

        [Fact]
        public async Task GetSizeAsync_ShouldReturnNull_WhenSizeDoesNotExist()
        {
            // Arrange
            var sizeService = new SizeService(_inMemorySizeRepository, _mockLogger.Object);
            var productSize = new ProductSize { SizeType = "NonExistentType", SizeValue = "NonExistentValue", AgeGroup = "NonExistentGroup" };

            // Act
            var result = await sizeService.GetSizeAsync(productSize);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateSizeAsync_ShouldUpdateSize_WhenSizeExists()
        {
            // Arrange
            var sizeService = new SizeService(_inMemorySizeRepository, _mockLogger.Object);
            var productSize = new ProductSize { SizeType = "Small", SizeValue = "S", AgeGroup = "Adult" };
            var existingSize = new SizeEntity { SizeType = productSize.SizeType, SizeValue = productSize.SizeValue, AgeGroup = productSize.AgeGroup };
            await _inMemorySizeRepository.AddAsync(existingSize);

            // Act
            existingSize.SizeValue = "M";
            var result = await sizeService.UpdateSizeAsync(existingSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("M", result.SizeValue);
        }

        [Fact]
        public async Task GetAllSizesAsync_ShouldReturnAllSizes_WhenSizesExist()
        {
            // Arrange
            var sizeService = new SizeService(_inMemorySizeRepository, _mockLogger.Object);
            var testData = new List<SizeEntity>
            {
                new() { SizeType = "Small", SizeValue = "S", AgeGroup = "Adult" },
                new() { SizeType = "Medium", SizeValue = "M", AgeGroup = "Adult" }
            };

            foreach (var size in testData)
            {
                await _inMemorySizeRepository.AddAsync(size);
            }

            // Act
            var result = await sizeService.GetAllSizesAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(testData.Count, result.Count());
        }

        [Fact]
        public async Task GetSizeAsync_ShouldReturnSize_WhenIdExists()
        {
            // Arrange
            var sizeService = new SizeService(_inMemorySizeRepository, _mockLogger.Object);
            var existingSize = new SizeEntity { SizeType = "Small", SizeValue = "S", AgeGroup = "Adult" };
            await _inMemorySizeRepository.AddAsync(existingSize);

            // Act
            var result = await sizeService.GetSizeAsync(existingSize.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingSize, result);
        }

        [Fact]
        public async Task GetSizeAsync_ShouldReturnNull_WhenIdDoesNotExist()
        {
            // Arrange
            var sizeService = new SizeService(_inMemorySizeRepository, _mockLogger.Object);

            // Act
            var result = await sizeService.GetSizeAsync(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteSizeAsync_ShouldDeleteSize_WhenSizeExists()
        {
            // Arrange
            var sizeService = new SizeService(_inMemorySizeRepository, _mockLogger.Object);
            var productSize = new ProductSize { SizeType = "ToDelete", SizeValue = "S", AgeGroup = "Adult" };
            var sizeToDelete = new SizeEntity { SizeType = productSize.SizeType, SizeValue = productSize.SizeValue, AgeGroup = productSize.AgeGroup };
            await _inMemorySizeRepository.AddAsync(sizeToDelete);

            // Act
            var result = await sizeService.DeleteSizeAsync(productSize);

            // Assert
            Assert.True(result);

        }

        [Fact]
        public async Task DeleteSizeAsync_ShouldNotDeleteSize_WhenSizeDoesNotExist()
        {
            // Arrange
            var sizeService = new SizeService(_inMemorySizeRepository, _mockLogger.Object);
            var productSize = new ProductSize { SizeType = "NonExistentType", SizeValue = "NonExistentValue", AgeGroup = "NonExistentGroup" };

            // Act
            var result = await sizeService.DeleteSizeAsync(productSize);

            // Assert
            Assert.False(result);
        }
    }

    public class InMemorySizeRepository : SizeRepository
    {
        private readonly List<SizeEntity> _sizes;

        public InMemorySizeRepository(ProductDataContext context) : base(context)
        {
            _sizes = new List<SizeEntity>();
        }

        public override Task<IEnumerable<SizeEntity>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<SizeEntity>>(_sizes);
        }

        public override Task<SizeEntity> GetOneAsync(Expression<Func<SizeEntity, bool>> predicate)
        {
            return Task.FromResult(_sizes.AsQueryable().FirstOrDefault(predicate.Compile())!);
        }

        public override Task<SizeEntity> AddAsync(SizeEntity entity)
        {
            _sizes.Add(entity);
            return Task.FromResult(entity);
        }

        public override Task<SizeEntity> UpdateAsync(Expression<Func<SizeEntity, bool>> predicate, SizeEntity entity)
        {
            var existingSize = _sizes.AsQueryable().FirstOrDefault(predicate.Compile());
            if (existingSize != null)
            {
                existingSize.SizeType = entity.SizeType;
                existingSize.SizeValue = entity.SizeValue;
                existingSize.AgeGroup = entity.AgeGroup;
            }
            return Task.FromResult(existingSize!);
        }

        public override Task<bool> RemoveAsync(Expression<Func<SizeEntity, bool>> predicate)
        {
            var entityToRemove = _sizes.FirstOrDefault(predicate.Compile());
            if (entityToRemove != null)
            {
                _sizes.Remove(entityToRemove);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
