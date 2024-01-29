using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests;

public class ProductImageRepositoryTests
{
    private readonly ProductDataContext _context =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

    [Fact]
    public async Task AddAsync_Should_Add_ProductImageEntity_Successfully()
    {
        // Arrange
        var repository = new ProductImageRepository(_context);

        var productImageEntity = new ProductImageEntity
        {
            ProductVariantId = 1,
            ArticleNumber = "ABC123",
            ImageId = 1
        };

        // Act 
        var result = await repository.AddAsync(productImageEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productImageEntity.ProductVariantId, result.ProductVariantId);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_ProductImageEntities_With_RelatedEntities()
    {
        // Arrange
        var repository = new ProductImageRepository(_context);

        // Seed data
        var testData = new List<ProductImageEntity>
        {
            new() { ProductVariantId = 1, ArticleNumber = "ABC123", ImageId = 1, Image = new ImageEntity{Id =1, ImageUrl = "link"} },
            new() { ProductVariantId = 2, ArticleNumber = "XYZ456", ImageId = 2, Image = new ImageEntity{Id =2, ImageUrl = "link2"}  }
        };

        _context.ProductImageEntities.AddRange(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());

        foreach (var entity in result)
        {
            Assert.NotNull(entity.Image);
        }
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_ProductImageEntity_Successfully()
    {
        // Arrange
        var repository = new ProductImageRepository(_context);

        var productImageEntity = new ProductImageEntity
        {
            ProductVariantId = 1,
            ArticleNumber = "ABC123",
            ImageId = 1
        };

        await repository.AddAsync(productImageEntity);

        productImageEntity.ArticleNumber = "UpdatedArticleNumber";

        // Act
        var result = await repository.UpdateAsync(pi => pi.Id == productImageEntity.Id, productImageEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UpdatedArticleNumber", result.ArticleNumber);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_ProductImageEntity_Successfully()
    {
        // Arrange
        var repository = new ProductImageRepository(_context);

        var productImageEntity = new ProductImageEntity
        {
            ProductVariantId = 1,
            ArticleNumber = "ABC123",
            ImageId = 1
        };

        await repository.AddAsync(productImageEntity);

        // Act
        var result = await repository.RemoveAsync(pi => pi.Id == productImageEntity.Id);

        // Assert
        Assert.True(result);

        var removedProductImage = await repository.GetOneAsync(pi => pi.Id == productImageEntity.Id);
        Assert.Null(removedProductImage);
    }

    [Fact]
    public async Task GetOneAsync_Should_Return_ProductImageEntity_With_RelatedEntities()
    {
        // Arrange
        var repository = new ProductImageRepository(_context);

        // Seed data
        var productImageEntity = new ProductImageEntity { ProductVariantId = 1, ArticleNumber = "ABC123", ImageId = 1, Image = new ImageEntity { Id = 1, ImageUrl = "link" } };
        _context.ProductImageEntities.Add(productImageEntity);
        _context.SaveChanges();

        // Act
        var result = await repository.GetOneAsync(pi => pi.Id == productImageEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Image);
    }
}
