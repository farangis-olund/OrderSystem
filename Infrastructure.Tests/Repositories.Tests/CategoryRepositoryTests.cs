using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests;

public class CategoryRepositoryTests
{
    private readonly ProductDataContext _context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task AddAsync_Should_Add_CategoryEntity_Successfully()
    {
        // Arrange
        var repository = new CategoryRepository(_context);

        var entity = new CategoryEntity { CategoryName = "CategoryTest" };

        // Act 
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.CategoryName, result.CategoryName);
    }

    [Fact]
    public async Task AddAsync_Should_Not_Add_Duplicate_CategoryEntity()
    {
        // Arrange
        var repository = new CategoryRepository(_context);

        var entity = new CategoryEntity { CategoryName = "CategoryTest" };

        await repository.AddAsync(entity);

        // Act
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_CategoryEntities()
    {
        // Arrange
        var repository = new CategoryRepository(_context);
        var testData = new List<CategoryEntity>
        {
            new CategoryEntity { CategoryName = "Test1" },
            new CategoryEntity { CategoryName = "Test2" }
        };

        _context.CategoryEntities.AddRange(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_CategoryEntity_Successfully()
    {
        // Arrange
        var repository = new CategoryRepository(_context);
        var entity = new CategoryEntity { CategoryName = "CategoryTest" };

        await repository.AddAsync(entity);
        entity.CategoryName = "UpdatedCategoryName";

        // Act
        var result = await repository.UpdateAsync(c => c.Id == entity.Id, entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UpdatedCategoryName", result.CategoryName);
    }

    [Fact]
    public async Task UpdateAsync_Should_Not_Update_Nonexistent_CategoryEntity()
    {
        // Arrange
        var repository = new CategoryRepository(_context);
        var entity = new CategoryEntity { CategoryName = "CategoryTest" };

        entity.CategoryName = "UpdatedCategoryName";

        // Act
        var result = await repository.UpdateAsync(c => c.Id == entity.Id, entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_CategoryEntity_Successfully()
    {
        // Arrange
        var repository = new CategoryRepository(_context);
        var entity = new CategoryEntity { CategoryName = "CategoryTest" };
        await repository.AddAsync(entity);

        // Act
        var result = await repository.RemoveAsync(c => c.Id == entity.Id);

        // Assert
        Assert.True(result);

        var removedCategory = await repository.GetOneAsync(c => c.Id == entity.Id);
        Assert.Null(removedCategory);
    }

    [Fact]
    public async Task RemoveAsync_Should_Not_Remove_Nonexistent_CategoryEntity()
    {
        // Arrange
        var repository = new CategoryRepository(_context);
        var entity = new CategoryEntity { CategoryName = "CategoryTest" };

        // Act
        var result = await repository.RemoveAsync(c => c.Id == entity.Id);

        // Assert
        Assert.False(result);
    }
}
