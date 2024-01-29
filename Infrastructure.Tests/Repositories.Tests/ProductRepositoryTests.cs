using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests;

public class ProductRepositoryTests
{
    private readonly ProductDataContext _context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task AddAsync_Should_Add_ProductEntity_Successfully()
    {
        // Arrange
        var repository = new ProductRepository(_context);
        var entity = new ProductEntity
        {
            ArticleNumber = "NewProduct",
            ProductName = "NewProductName",
            Material = "Material",
            ProductInfo = "ProductInfo",
            CategoryId = 1,
            BrandId = 1,
        };

        // Act 
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewProduct", result.ArticleNumber);
        Assert.Equal("NewProductName", result.ProductName);
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_All_ProductEntities_With_Brand_And_Category()
    {
        // Arrange
        var repository = new ProductRepository(_context);
        var testData = new List<ProductEntity>
        {
            new() {
                ArticleNumber = "ABC123",
                ProductName = "Product1",
                Material = "Material1",
                ProductInfo = "Info1",
                CategoryId = 1,
                BrandId = 1,
                Brand = new BrandEntity { Id = 1, BrandName = "Brand1" },
                Category = new CategoryEntity { Id = 1, CategoryName = "Category1" }
            },
            new() {
                ArticleNumber = "XYZ456",
                ProductName = "Product2",
                Material = "Material2",
                ProductInfo = "Info2",
                CategoryId = 2,
                BrandId = 2,
                Brand = new BrandEntity { Id = 2, BrandName = "Brand2" },
                Category = new CategoryEntity { Id = 2, CategoryName = "Category2" }
            }
        };

        _context.ProductEntities.AddRange(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());

        foreach (var entity in result)
        {
            Assert.NotNull(entity.Brand);
            Assert.NotNull(entity.Category);
            Assert.NotEmpty(entity.Brand.BrandName);
            Assert.NotEmpty(entity.Category.CategoryName);
        }
    }

    [Fact]
    public async Task GetOneAsync_Should_Return_ProductEntity_With_Brand_And_Category()
    {
        // Arrange
        var repository = new ProductRepository(_context);
        var testData = new ProductEntity
        {
            ArticleNumber = "ABC123",
            ProductName = "Product1",
            Material = "Material1",
            ProductInfo = "Info1",
            CategoryId = 1,
            BrandId = 1,
            Brand = new BrandEntity { Id = 1, BrandName = "Brand1" },
            Category = new CategoryEntity { Id = 1, CategoryName = "Category1" }
        };

        _context.ProductEntities.Add(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetOneAsync(p => p.ArticleNumber == testData.ArticleNumber);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Brand);
        Assert.NotNull(result.Category);
        Assert.NotEmpty(result.Brand.BrandName);
        Assert.NotEmpty(result.Category.CategoryName);
    }

    [Fact]
    public async Task GetOneAsync_Should_Create_If_Not_Found_And_Return_ProductEntity_With_Brand_And_Category()
    {
        // Arrange
        var repository = new ProductRepository(_context);

        // Act
        var result = await repository.GetOneAsync(p => p.ArticleNumber == "NonExistent", async () =>
        {
            var newEntity = new ProductEntity
            {
                ArticleNumber = "NonExistent",
                ProductName = "Product1",
                Material = "Material1",
                ProductInfo = "Info1",
                CategoryId = 1,
                BrandId = 1,
                Brand = new BrandEntity { Id = 1, BrandName = "Brand1" },
                Category = new CategoryEntity { Id = 1, CategoryName = "Category1" }
            };

            await repository.AddAsync(newEntity);
            return newEntity;
        });

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Brand);
        Assert.NotNull(result.Category);
        Assert.NotEmpty(result.Brand.BrandName);
        Assert.NotEmpty(result.Category.CategoryName);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_ProductEntity_Successfully()
    {
        // Arrange
        var repository = new ProductRepository(_context);
        var entity = new ProductEntity
        {
            ArticleNumber = "ExistingProduct",
            ProductName = "ExistingProductName",
            Material = "Material",
            ProductInfo = "ProductInfo",
            CategoryId = 1,
            BrandId = 1,
        };

        await repository.AddAsync(entity);
        entity.ProductName = "UpdatedProductName";

        // Act
        var result = await repository.UpdateAsync(p => p.ArticleNumber == entity.ArticleNumber, entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UpdatedProductName", result.ProductName);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_ProductEntity_Successfully()
    {
        // Arrange
        var repository = new ProductRepository(_context);
        var entity = new ProductEntity
        {
            ArticleNumber = "ExistingProduct",
            ProductName = "ExistingProductName",
            Material = "Material",
            ProductInfo = "ProductInfo",
            CategoryId = 1,
            BrandId = 1,
        };

        await repository.AddAsync(entity);

        // Act
        var result = await repository.RemoveAsync(p => p.ArticleNumber == entity.ArticleNumber);

        // Assert
        Assert.True(result);

        var removedProduct = await repository.GetOneAsync(p => p.ArticleNumber == entity.ArticleNumber);
        Assert.Null(removedProduct);
    }

    [Fact]
    public async Task RemoveAsync_Should_Not_Remove_Nonexistent_ProductEntity()
    {
        // Arrange
        var repository = new ProductRepository(_context);
        var entity = new ProductEntity
        {
            ArticleNumber = "NonExistentProduct",
            ProductName = "NonExistentProductName",
            Material = "Material",
            ProductInfo = "ProductInfo",
            CategoryId = 1,
            BrandId = 1,
        };

        // Act
        var result = await repository.RemoveAsync(p => p.ArticleNumber == entity.ArticleNumber);

        // Assert
        Assert.False(result);
    }
}
