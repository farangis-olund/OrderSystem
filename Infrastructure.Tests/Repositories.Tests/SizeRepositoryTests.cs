using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Tests;

public class SizeRepositoryTests
{
    private readonly ProductDataContext _context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task AddAsync_Should_Add_SizeEntity_Successfully()
    {
        // Arrange
        var repository = new SizeRepository(_context);

        var entity = new SizeEntity
        {
            SizeType = "Shirt",
            SizeValue = "M",
            AgeGroup = "Adult"
        };

        // Act 
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Shirt", result.SizeType);
        Assert.Equal("M", result.SizeValue);
        Assert.Equal("Adult", result.AgeGroup);
        Assert.NotEqual(default(int), result.Id);
    }

    [Fact]
    public async Task AddAsync_Should_Not_Add_Duplicate_SizeEntity()
    {
        // Arrange
        var repository = new SizeRepository(_context);

        var entity = new SizeEntity
        {
            SizeType = "Shirt",
            SizeValue = "M",
            AgeGroup = "Adult"
        };

        await repository.AddAsync(entity);

        // Act
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_SizeEntities()
    {
        // Arrange
        var repository = new SizeRepository(_context);
        var testData = new List<SizeEntity>
        {
            new() { SizeType = "Shirt", SizeValue = "M", AgeGroup = "Adult" },
            new() { SizeType = "Pants", SizeValue = "L", AgeGroup = "Child" }
        };

        _context.SizeEntities.AddRange(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_SizeEntity_Successfully()
    {
        // Arrange
        var repository = new SizeRepository(_context);
        var entity = new SizeEntity { SizeType = "Shirt", SizeValue = "M", AgeGroup = "Adult" };

        await repository.AddAsync(entity);
        entity.SizeValue = "L";

        // Act
        var result = await repository.UpdateAsync(s => s.Id == entity.Id, entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Shirt", result.SizeType);
        Assert.Equal("L", result.SizeValue);
        Assert.Equal("Adult", result.AgeGroup);
    }

    [Fact]
    public async Task UpdateAsync_Should_Not_Update_Nonexistent_SizeEntity()
    {
        // Arrange
        var repository = new SizeRepository(_context);
        var entity = new SizeEntity { SizeType = "Shirt", SizeValue = "M", AgeGroup = "Adult" };

        entity.SizeValue = "L";

        // Act
        var result = await repository.UpdateAsync(s => s.Id == entity.Id, entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_SizeEntity_Successfully()
    {
        // Arrange
        var repository = new SizeRepository(_context);
        var entity = new SizeEntity { SizeType = "Shirt", SizeValue = "M", AgeGroup = "Adult" };
        await repository.AddAsync(entity);

        // Act
        var result = await repository.RemoveAsync(s => s.Id == entity.Id);

        // Assert
        Assert.True(result);

        var removedSize = await repository.GetOneAsync(s => s.Id == entity.Id);
        Assert.Null(removedSize);
    }

    [Fact]
    public async Task RemoveAsync_Should_Not_Remove_Nonexistent_SizeEntity()
    {
        // Arrange
        var repository = new SizeRepository(_context);
        var entity = new SizeEntity { SizeType = "Shirt", SizeValue = "M", AgeGroup = "Adult" };

        // Act
        var result = await repository.RemoveAsync(s => s.Id == entity.Id);

        // Assert
        Assert.False(result);
    }
}
