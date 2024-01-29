using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests;

public class ImageRepositoryTests
{
    private readonly ProductDataContext _context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task AddAsync_Should_Add_ImageEntity_Successfully()
    {
        // Arrange
        var repository = new ImageRepository(_context);

        var entity = new ImageEntity { ImageUrl = "https://example.com/image.jpg" };

        // Act 
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("https://example.com/image.jpg", result.ImageUrl);
        Assert.NotEqual(default(int), result.Id);
    }

    [Fact]
    public async Task AddAsync_Should_Not_Add_Duplicate_ImageEntity()
    {
        // Arrange
        var repository = new ImageRepository(_context);

        var entity = new ImageEntity { ImageUrl = "https://example.com/image.jpg" };

        await repository.AddAsync(entity);

        // Act
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_ImageEntities()
    {
        // Arrange
        var repository = new ImageRepository(_context);
        var testData = new List<ImageEntity>
        {
            new ImageEntity { ImageUrl = "https://example.com/image1.jpg" },
            new ImageEntity { ImageUrl = "https://example.com/image2.jpg" }
        };

        _context.ImageEntities.AddRange(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_ImageEntity_Successfully()
    {
        // Arrange
        var repository = new ImageRepository(_context);
        var entity = new ImageEntity { ImageUrl = "https://example.com/image.jpg" };

        await repository.AddAsync(entity);
        entity.ImageUrl = "https://example.com/updated-image.jpg";

        // Act
        var result = await repository.UpdateAsync(i => i.Id == entity.Id, entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("https://example.com/updated-image.jpg", result.ImageUrl);
    }

    [Fact]
    public async Task UpdateAsync_Should_Not_Update_Nonexistent_ImageEntity()
    {
        // Arrange
        var repository = new ImageRepository(_context);
        var entity = new ImageEntity { ImageUrl = "https://example.com/image.jpg" };

        entity.ImageUrl = "https://example.com/updated-image.jpg";

        // Act
        var result = await repository.UpdateAsync(i => i.Id == entity.Id, entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_ImageEntity_Successfully()
    {
        // Arrange
        var repository = new ImageRepository(_context);
        var entity = new ImageEntity { ImageUrl = "https://example.com/image.jpg" };
        await repository.AddAsync(entity);

        // Act
        var result = await repository.RemoveAsync(i => i.Id == entity.Id);

        // Assert
        Assert.True(result);

        var removedImage = await repository.GetOneAsync(i => i.Id == entity.Id);
        Assert.Null(removedImage);
    }

    [Fact]
    public async Task RemoveAsync_Should_Not_Remove_Nonexistent_ImageEntity()
    {
        // Arrange
        var repository = new ImageRepository(_context);
        var entity = new ImageEntity { ImageUrl = "https://example.com/image.jpg" };

        // Act
        var result = await repository.RemoveAsync(i => i.Id == entity.Id);

        // Assert
        Assert.False(result);
    }
}
