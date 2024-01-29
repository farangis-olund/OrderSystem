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
    public class BrandServiceTests
    {
        private readonly InMemoryBrandRepository _inMemoryBrandRepository;
        private readonly Mock<ILogger<BrandService>> _mockLogger;

        public BrandServiceTests()
        {
            var context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);
            _inMemoryBrandRepository = new InMemoryBrandRepository(context);
            _mockLogger = new Mock<ILogger<BrandService>>();
        }

        [Fact]
        public async Task AddBrandAsync_ShouldAddBrand_WhenBrandDoesNotExist()
        {
            // Arrange
            var brandService = new BrandService(_inMemoryBrandRepository, _mockLogger.Object);
            var brandName = "NewBrand";

            // Act
            var result = await brandService.AddBrandAsync(brandName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(brandName, result.BrandName);
        }

        [Fact]
        public async Task AddBrandAsync_ShouldReturnExistingBrand_WhenBrandExists()
        {
            // Arrange
            var brandService = new BrandService(_inMemoryBrandRepository, _mockLogger.Object);
            var brandName = "ExistingBrand";
            var existingBrand = new BrandEntity { BrandName = brandName };
            await _inMemoryBrandRepository.AddAsync(existingBrand);

            // Act
            var result = await brandService.AddBrandAsync(brandName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingBrand, result);
        }

        [Fact]
        public async Task GetBrandAsync_ShouldReturnBrand_WhenBrandExists()
        {
            // Arrange
            var brandService = new BrandService(_inMemoryBrandRepository, _mockLogger.Object);
            var brandName = "ExistingBrand";
            var existingBrand = new BrandEntity { BrandName = brandName };
            await _inMemoryBrandRepository.AddAsync(existingBrand);

            // Act
            var result = await brandService.GetBrandAsync(brandName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingBrand, result);
        }

        [Fact]
        public async Task GetBrandAsync_ShouldReturnNull_WhenBrandDoesNotExist()
        {
            // Arrange
            var brandService = new BrandService(_inMemoryBrandRepository, _mockLogger.Object);
            var brandName = "NonExistentBrand";

            // Act
            var result = await brandService.GetBrandAsync(brandName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateBrandAsync_ShouldUpdateBrand_WhenBrandExists()
        {
            // Arrange
            var brandService = new BrandService(_inMemoryBrandRepository, _mockLogger.Object);
            var brandName = "ExistingBrand";
            var existingBrand = new BrandEntity { BrandName = brandName };
            await _inMemoryBrandRepository.AddAsync(existingBrand);

            // Act
            existingBrand.BrandName = "UpdatedBrandName";
            var result = await brandService.UpdateBrandAsync(existingBrand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedBrandName", result.BrandName);
        }

        [Fact]
        public async Task GetAllBrandsAsync_ShouldReturnAllBrands_WhenBrandsExist()
        {
            // Arrange
            var brandService = new BrandService(_inMemoryBrandRepository, _mockLogger.Object);
            var testData = new List<BrandEntity>
    {
        new() { BrandName = "Test1" },
        new() { BrandName = "Test2" }
    };

            foreach (var brand in testData)
            {
                await _inMemoryBrandRepository.AddAsync(brand);
            }

            // Act
            var result = await brandService.GetAllBrandsAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(testData.Count, result.Count());
        }

        [Fact]
        public async Task DeleteBrandAsync_ShouldDeleteBrand_WhenBrandExists()
        {
            // Arrange
            var brandService = new BrandService(_inMemoryBrandRepository, _mockLogger.Object);
            var brandName = "BrandToDelete";
            var brandToDelete = new BrandEntity { BrandName = brandName };
            await _inMemoryBrandRepository.AddAsync(brandToDelete);

            // Act
            var result = await brandService.DeleteBrandAsync(brandName);

            // Assert
            Assert.True(result);

            var deletedBrand = await _inMemoryBrandRepository.GetOneAsync(b => b.BrandName == brandName);
            Assert.Null(deletedBrand);
        }

        [Fact]
        public async Task DeleteBrandAsync_ShouldNotDeleteBrand_WhenBrandDoesNotExist()
        {
            // Arrange
            var brandService = new BrandService(_inMemoryBrandRepository, _mockLogger.Object);
            var brandName = "NonExistentBrand";

            // Act
            var result = await brandService.DeleteBrandAsync(brandName);

            // Assert
            Assert.False(result);
        }

    }

    public class InMemoryBrandRepository : BrandRepository
    {
        private readonly List<BrandEntity> _brands;

        public InMemoryBrandRepository(ProductDataContext context) : base(context)
        {
            _brands = new List<BrandEntity>();
        }

        public override Task<IEnumerable<BrandEntity>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<BrandEntity>>(_brands);
        }

        public override Task<BrandEntity> GetOneAsync(Expression<Func<BrandEntity, bool>> predicate)
        {
            return Task.FromResult(_brands.AsQueryable().FirstOrDefault(predicate.Compile())!);
        }

        public override Task<BrandEntity> AddAsync(BrandEntity entity)
        {
            _brands.Add(entity);
            return Task.FromResult(entity);
        }

        public override Task<BrandEntity> UpdateAsync(Expression<Func<BrandEntity, bool>> predicate, BrandEntity entity)
        {
            var existingBrand = _brands.AsQueryable().FirstOrDefault(predicate.Compile());
            if (existingBrand != null)
            {
                existingBrand.BrandName = entity.BrandName;
            }
            return Task.FromResult(existingBrand!);
        }

        public override Task<bool> RemoveAsync(Expression<Func<BrandEntity, bool>> predicate)
        {
            var entityToRemove = _brands.FirstOrDefault(predicate.Compile());
            if (entityToRemove != null)
            {
                _brands.Remove(entityToRemove);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }


}
