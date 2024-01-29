using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests;

public class ColorRepositoryTests
{
    private readonly ProductDataContext _context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task AddAsync_Should_Add_ColorEntity_Successfully()
    {
        // Arrange
        var repository = new ColorRepository(_context);

        var entity = new ColorEntity { ColorName = "ColorTest" };

        // Act 
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.ColorName, result.ColorName);
    }

    [Fact]
    public async Task AddAsync_Should_Not_Add_Duplicate_ColorEntity()
    {
        // Arrange
        var repository = new ColorRepository(_context);

        var entity = new ColorEntity { ColorName = "ColorTest" };

        await repository.AddAsync(entity);

        // Act
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_ColorEntities()
    {
        // Arrange
        var repository = new ColorRepository(_context);
        var testData = new List<ColorEntity>
        {
            new ColorEntity { ColorName = "Test1" },
            new ColorEntity { ColorName = "Test2" }
        };

        _context.ColorEntities.AddRange(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_ColorEntity_Successfully()
    {
        // Arrange
        var repository = new ColorRepository(_context);
        var entity = new ColorEntity { ColorName = "ColorTest" };

        await repository.AddAsync(entity);
        entity.ColorName = "UpdatedColorName";

        // Act
        var result = await repository.UpdateAsync(c => c.Id == entity.Id, entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UpdatedColorName", result.ColorName);
    }

    [Fact]
    public async Task UpdateAsync_Should_Not_Update_Nonexistent_ColorEntity()
    {
        // Arrange
        var repository = new ColorRepository(_context);
        var entity = new ColorEntity { ColorName = "ColorTest" };

        entity.ColorName = "UpdatedColorName";

        // Act
        var result = await repository.UpdateAsync(c => c.Id == entity.Id, entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_ColorEntity_Successfully()
    {
        // Arrange
        var repository = new ColorRepository(_context);
        var entity = new ColorEntity { ColorName = "ColorTest" };
        await repository.AddAsync(entity);

        // Act
        var result = await repository.RemoveAsync(c => c.Id == entity.Id);

        // Assert
        Assert.True(result);

        var removedColor = await repository.GetOneAsync(c => c.Id == entity.Id);
        Assert.Null(removedColor);
    }

    [Fact]
    public async Task RemoveAsync_Should_Not_Remove_Nonexistent_ColorEntity()
    {
        // Arrange
        var repository = new ColorRepository(_context);
        var entity = new ColorEntity { ColorName = "ColorTest" };

        // Act
        var result = await repository.RemoveAsync(c => c.Id == entity.Id);

        // Assert
        Assert.False(result);
    }
}

