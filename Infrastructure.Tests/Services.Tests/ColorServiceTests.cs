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
    public class ColorServiceTests
    {
        private readonly InMemoryColorRepository _inMemoryColorRepository;
        private readonly Mock<ILogger<ColorService>> _mockLogger;

        public ColorServiceTests()
        {
            var context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _inMemoryColorRepository = new InMemoryColorRepository(context);
            _mockLogger = new Mock<ILogger<ColorService>>();
        }

        [Fact]
        public async Task AddColorAsync_ShouldAddColor_WhenColorDoesNotExist()
        {
            // Arrange
            var colorService = new ColorService(_inMemoryColorRepository, _mockLogger.Object);
            var colorName = "NewColor";

            // Act
            var result = await colorService.AddColorAsync(colorName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(colorName, result.ColorName);
        }

        [Fact]
        public async Task AddColorAsync_ShouldReturnExistingColor_WhenColorExists()
        {
            // Arrange
            var colorService = new ColorService(_inMemoryColorRepository, _mockLogger.Object);
            var colorName = "ExistingColor";
            var existingColor = new ColorEntity { ColorName = colorName };
            await _inMemoryColorRepository.AddAsync(existingColor);

            // Act
            var result = await colorService.AddColorAsync(colorName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingColor, result);
        }

        [Fact]
        public async Task GetColorAsync_ShouldReturnColor_WhenColorExists()
        {
            // Arrange
            var colorService = new ColorService(_inMemoryColorRepository, _mockLogger.Object);
            var colorName = "ExistingColor";
            var existingColor = new ColorEntity { ColorName = colorName };
            await _inMemoryColorRepository.AddAsync(existingColor);

            // Act
            var result = await colorService.GetColorAsync(colorName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingColor, result);
        }

        [Fact]
        public async Task GetColorAsync_ShouldReturnNull_WhenColorDoesNotExist()
        {
            // Arrange
            var colorService = new ColorService(_inMemoryColorRepository, _mockLogger.Object);
            var colorName = "NonExistentColor";

            // Act
            var result = await colorService.GetColorAsync(colorName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateColorAsync_ShouldUpdateColor_WhenColorExists()
        {
            // Arrange
            var colorService = new ColorService(_inMemoryColorRepository, _mockLogger.Object);
            var colorName = "ExistingColor";
            var existingColor = new ColorEntity { ColorName = colorName };
            await _inMemoryColorRepository.AddAsync(existingColor);

            // Act
            existingColor.ColorName = "UpdatedColorName";
            var result = await colorService.UpdateColorAsync(existingColor);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedColorName", result.ColorName);
        }

        [Fact]
        public async Task GetAllColorsAsync_ShouldReturnAllColors_WhenColorsExist()
        {
            // Arrange
            var colorService = new ColorService(_inMemoryColorRepository, _mockLogger.Object);
            var testData = new List<ColorEntity>
            {
                new() { ColorName = "Test1" },
                new() { ColorName = "Test2" }
            };

            foreach (var color in testData)
            {
                await _inMemoryColorRepository.AddAsync(color);
            }

            // Act
            var result = await colorService.GetAllColorsAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(testData.Count, result.Count());
        }

        [Fact]
        public async Task DeleteColorAsync_ShouldDeleteColor_WhenColorExists()
        {
            // Arrange
            var colorService = new ColorService(_inMemoryColorRepository, _mockLogger.Object);
            var colorName = "ColorToDelete";
            var colorToDelete = new ColorEntity { ColorName = colorName };
            await _inMemoryColorRepository.AddAsync(colorToDelete);

            // Act
            var result = await colorService.DeleteColorAsync(colorName);

            // Assert
            Assert.True(result);

            var deletedColor = await _inMemoryColorRepository.GetOneAsync(c => c.ColorName == colorName);
            Assert.Null(deletedColor);
        }

        [Fact]
        public async Task DeleteColorAsync_ShouldNotDeleteColor_WhenColorDoesNotExist()
        {
            // Arrange
            var colorService = new ColorService(_inMemoryColorRepository, _mockLogger.Object);
            var colorName = "NonExistentColor";

            // Act
            var result = await colorService.DeleteColorAsync(colorName);

            // Assert
            Assert.False(result);
        }
    }

    public class InMemoryColorRepository : ColorRepository
    {
        private readonly List<ColorEntity> _colors;

        public InMemoryColorRepository(ProductDataContext context) : base(context)
        {
            _colors = new List<ColorEntity>();
        }

        public override Task<IEnumerable<ColorEntity>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<ColorEntity>>(_colors);
        }

        public override Task<ColorEntity> GetOneAsync(Expression<Func<ColorEntity, bool>> predicate)
        {
            return Task.FromResult(_colors.AsQueryable().FirstOrDefault(predicate.Compile())!);
        }

        public override Task<ColorEntity> AddAsync(ColorEntity entity)
        {
            _colors.Add(entity);
            return Task.FromResult(entity);
        }

        public override Task<ColorEntity> UpdateAsync(Expression<Func<ColorEntity, bool>> predicate, ColorEntity entity)
        {
            var existingColor = _colors.AsQueryable().FirstOrDefault(predicate.Compile());
            if (existingColor != null)
            {
                existingColor.ColorName = entity.ColorName;
            }
            return Task.FromResult(existingColor!);
        }

        public override Task<bool> RemoveAsync(Expression<Func<ColorEntity, bool>> predicate)
        {
            var entityToRemove = _colors.FirstOrDefault(predicate.Compile());
            if (entityToRemove != null)
            {
                _colors.Remove(entityToRemove);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
