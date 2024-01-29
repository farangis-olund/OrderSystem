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
    public class CategoryServiceTests
    {
        private readonly InMemoryCategoryRepository _inMemoryCategoryRepository;
        private readonly Mock<ILogger<CategoryService>> _mockLogger;

        public CategoryServiceTests()
        {
            var context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _inMemoryCategoryRepository = new InMemoryCategoryRepository(context);
            _mockLogger = new Mock<ILogger<CategoryService>>();
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldAddCategory_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryService = new CategoryService(_inMemoryCategoryRepository, _mockLogger.Object);
            var categoryName = "NewCategory";

            // Act
            var result = await categoryService.AddCategoryAsync(categoryName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryName, result.CategoryName);
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldReturnExistingCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryService = new CategoryService(_inMemoryCategoryRepository, _mockLogger.Object);
            var categoryName = "ExistingCategory";
            var existingCategory = new CategoryEntity { CategoryName = categoryName };
            await _inMemoryCategoryRepository.AddAsync(existingCategory);

            // Act
            var result = await categoryService.AddCategoryAsync(categoryName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingCategory, result);
        }

        [Fact]
        public async Task GetCategoryAsync_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryService = new CategoryService(_inMemoryCategoryRepository, _mockLogger.Object);
            var categoryName = "ExistingCategory";
            var existingCategory = new CategoryEntity { CategoryName = categoryName };
            await _inMemoryCategoryRepository.AddAsync(existingCategory);

            // Act
            var result = await categoryService.GetCategoryAsync(categoryName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingCategory, result);
        }

        [Fact]
        public async Task GetCategoryAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryService = new CategoryService(_inMemoryCategoryRepository, _mockLogger.Object);
            var categoryName = "NonExistentCategory";

            // Act
            var result = await categoryService.GetCategoryAsync(categoryName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryService = new CategoryService(_inMemoryCategoryRepository, _mockLogger.Object);
            var categoryName = "ExistingCategory";
            var existingCategory = new CategoryEntity { CategoryName = categoryName };
            await _inMemoryCategoryRepository.AddAsync(existingCategory);

            // Act
            existingCategory.CategoryName = "UpdatedCategoryName";
            var result = await categoryService.UpdateCategoryAsync(existingCategory);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedCategoryName", result.CategoryName);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnAllCategories_WhenCategoriesExist()
        {
            // Arrange
            var categoryService = new CategoryService(_inMemoryCategoryRepository, _mockLogger.Object);
            var testData = new List<CategoryEntity>
            {
                new() { CategoryName = "Test1" },
                new() { CategoryName = "Test2" }
            };

            foreach (var category in testData)
            {
                await _inMemoryCategoryRepository.AddAsync(category);
            }

            // Act
            var result = await categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(testData.Count, result.Count());
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldDeleteCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryService = new CategoryService(_inMemoryCategoryRepository, _mockLogger.Object);
            var categoryName = "CategoryToDelete";
            var categoryToDelete = new CategoryEntity { CategoryName = categoryName };
            await _inMemoryCategoryRepository.AddAsync(categoryToDelete);

            // Act
            var result = await categoryService.DeleteCategoryAsync(categoryName);

            // Assert
            Assert.True(result);

            var deletedCategory = await _inMemoryCategoryRepository.GetOneAsync(c => c.CategoryName == categoryName);
            Assert.Null(deletedCategory);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldNotDeleteCategory_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryService = new CategoryService(_inMemoryCategoryRepository, _mockLogger.Object);
            var categoryName = "NonExistentCategory";

            // Act
            var result = await categoryService.DeleteCategoryAsync(categoryName);

            // Assert
            Assert.False(result);
        }
    }

    public class InMemoryCategoryRepository : CategoryRepository
    {
        private readonly List<CategoryEntity> _categories;

        public InMemoryCategoryRepository(ProductDataContext context) : base(context)
        {
            _categories = new List<CategoryEntity>();
        }

        public override Task<IEnumerable<CategoryEntity>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<CategoryEntity>>(_categories);
        }

        public override Task<CategoryEntity> GetOneAsync(Expression<Func<CategoryEntity, bool>> predicate)
        {
            return Task.FromResult(_categories.AsQueryable().FirstOrDefault(predicate.Compile())!);
        }

        public override Task<CategoryEntity> AddAsync(CategoryEntity entity)
        {
            _categories.Add(entity);
            return Task.FromResult(entity);
        }

        public override Task<CategoryEntity> UpdateAsync(Expression<Func<CategoryEntity, bool>> predicate, CategoryEntity entity)
        {
            var existingCategory = _categories.AsQueryable().FirstOrDefault(predicate.Compile());
            if (existingCategory != null)
            {
                existingCategory.CategoryName = entity.CategoryName;
            }
            return Task.FromResult(existingCategory!);
        }

        public override Task<bool> RemoveAsync(Expression<Func<CategoryEntity, bool>> predicate)
        {
            var entityToRemove = _categories.FirstOrDefault(predicate.Compile());
            if (entityToRemove != null)
            {
                _categories.Remove(entityToRemove);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
