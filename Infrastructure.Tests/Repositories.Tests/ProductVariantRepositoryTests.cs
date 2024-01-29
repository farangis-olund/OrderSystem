using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;



namespace Infrastructure.Tests.Repositories.Tests;

public class ProductVariantRepositoryTests
{
    private readonly ProductDataContext _context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    


    [Fact]
    public async Task AddAsync_Should_Add_ProductVariantEntity_Successfully()
    {
        // Arrange
        var repository = new ProductVariantRepository(_context);
        var entity = new ProductVariantEntity
        {
            ArticleNumber = "NewProduct",
            Quantity = 10,
            SizeId = 1,
            ColorId = 1,
        };

        // Act 
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewProduct", result.ArticleNumber);
        Assert.Equal(10, result.Quantity);
    }

   
    [Fact]
    public async Task UpdateAsync_Should_Update_ProductVariantEntity_Successfully()
    {
        // Arrange
        var repository = new ProductVariantRepository(_context);
        var entity = new ProductVariantEntity
        {
            ArticleNumber = "ExistingProduct",
            Quantity = 5,
            SizeId = 1,
            ColorId = 1,
        };

        await repository.AddAsync(entity);
        entity.Quantity = 8;

        // Act
        var result = await repository.UpdateAsync(p => p.Id == entity.Id, entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(8, result.Quantity);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_ProductVariantEntity_Successfully()
    {
        // Arrange
        var repository = new ProductVariantRepository(_context);
        var entity = new ProductVariantEntity
        {
            ArticleNumber = "ExistingProduct",
            Quantity = 5,
            SizeId = 1,
            ColorId = 1,
        };

        await repository.AddAsync(entity);

        // Act
        var result = await repository.RemoveAsync(p => p.Id == entity.Id);

        // Assert
        Assert.True(result);

        var removedEntity = await repository.GetOneAsync(p => p.Id == entity.Id);
        Assert.Null(removedEntity);
    }

    [Fact]
    public async Task RemoveAsync_Should_Not_Remove_Nonexistent_ProductVariantEntity()
    {
        // Arrange
        var repository = new ProductVariantRepository(_context);
        var entity = new ProductVariantEntity
        {
            ArticleNumber = "NonExistentProduct",
            Quantity = 5,
            SizeId = 1,
            ColorId = 1,
        };

        // Act
        var result = await repository.RemoveAsync(p => p.Id == entity.Id);

        // Assert
        Assert.False(result);
    }
}
