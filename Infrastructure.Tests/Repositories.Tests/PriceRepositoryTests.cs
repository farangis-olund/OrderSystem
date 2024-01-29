using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests;

public class PriceRepositoryTests
{
    private readonly ProductDataContext _context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task AddAsync_Should_Add_ProductPriceEntity_Successfully()
    {
        // Arrange
        var repository = new PriceRepository(_context);

        var entity = new ProductPriceEntity
        {
            ProductVariantId = 1,
            ArticleNumber = "ABC123",
            Price = 100.00m,
            DiscountPrice = 80.00m,
            DicountPercentage = 20.00m,
            CurrencyCode = "USD"
        };

        // Act 
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ProductVariantId);
        Assert.Equal("ABC123", result.ArticleNumber);
        Assert.Equal(100.00m, result.Price);
        Assert.Equal(80.00m, result.DiscountPrice);
        Assert.Equal(20.00m, result.DicountPercentage);
        Assert.Equal("USD", result.CurrencyCode);
        Assert.NotEqual(default(int), result.Id);
    }

    [Fact]
    public async Task AddAsync_Should_Not_Add_Duplicate_ProductPriceEntity()
    {
        // Arrange
        var repository = new PriceRepository(_context);

        var entity = new ProductPriceEntity
        {
            ProductVariantId = 1,
            ArticleNumber = "ABC123",
            Price = 100.00m,
            DiscountPrice = 80.00m,
            DicountPercentage = 20.00m,
            CurrencyCode = "USD"
        };

        await repository.AddAsync(entity);

        // Act
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_ProductPriceEntities_With_Currency()
    {
        // Arrange
        var repository = new PriceRepository(_context);
        var testData = new List<ProductPriceEntity>
            {
                new ProductPriceEntity
                {
                    ProductVariantId = 1,
                    ArticleNumber = "ABC123",
                    Price = 100.00m,
                    DiscountPrice = 80.00m,
                    DicountPercentage = 20.00m,
                    CurrencyCode = "USD",
                    CurrencyCodeNavigation = new CurrencyEntity { Code = "USD", CurrencyName = "US Dollar" }
                },
                new ProductPriceEntity
                {
                    ProductVariantId = 2,
                    ArticleNumber = "XYZ456",
                    Price = 50.00m,
                    DiscountPrice = 40.00m,
                    DicountPercentage = 10.00m,
                    CurrencyCode = "EUR",
                    CurrencyCodeNavigation = new CurrencyEntity { Code = "EUR", CurrencyName = "Euro" }
                }
            };

        _context.ProductPriceEntities.AddRange(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());

        foreach (var entity in result)
        {
            Assert.NotNull(entity.CurrencyCodeNavigation);
            Assert.NotEmpty(entity.CurrencyCodeNavigation.CurrencyName);
        }
    }

    [Fact]
    public async Task GetOneAsync_Should_Return_ProductPriceEntity_With_Currency()
    {
        // Arrange
        var repository = new PriceRepository(_context);
        var testData = new ProductPriceEntity
        {
            ProductVariantId = 1,
            ArticleNumber = "ABC123",
            Price = 100.00m,
            DiscountPrice = 80.00m,
            DicountPercentage = 20.00m,
            CurrencyCode = "USD",
            CurrencyCodeNavigation = new CurrencyEntity { Code = "USD", CurrencyName = "US Dollar" }
        };

        _context.ProductPriceEntities.Add(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetOneAsync(p => p.Id == testData.Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.CurrencyCodeNavigation);
        Assert.Equal("USD", result.CurrencyCodeNavigation.Code);
        Assert.Equal("US Dollar", result.CurrencyCodeNavigation.CurrencyName);
    }

    [Fact]
    public async Task GetOneAsync_Should_Create_If_Not_Found_And_Return_ProductPriceEntity_With_Currency()
    {
        // Arrange
        var repository = new PriceRepository(_context);

        // Act
        var result = await repository.GetOneAsync(p => p.Id == 1, async () =>
        {
            var newEntity = new ProductPriceEntity
            {
                ProductVariantId = 1,
                ArticleNumber = "ABC123",
                Price = 100.00m,
                DiscountPrice = 80.00m,
                DicountPercentage = 20.00m,
                CurrencyCode = "USD",
                CurrencyCodeNavigation = new CurrencyEntity { Code = "USD", CurrencyName = "US Dollar" }
            };

            await repository.AddAsync(newEntity);
            return newEntity;
        });

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.CurrencyCodeNavigation);
        Assert.Equal("USD", result.CurrencyCodeNavigation.Code);
        Assert.Equal("US Dollar", result.CurrencyCodeNavigation.CurrencyName);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_ProductPriceEntity_Successfully()
    {
        // Arrange
        var repository = new PriceRepository(_context);
        var entity = new ProductPriceEntity
        {
            ProductVariantId = 1,
            ArticleNumber = "ABC123",
            Price = 100.00m,
            DiscountPrice = 80.00m,
            DicountPercentage = 20.00m,
            CurrencyCode = "USD"
        };

        await repository.AddAsync(entity);
        entity.Price = 120.00m;

        // Act
        var result = await repository.UpdateAsync(p => p.Id == entity.Id, entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ProductVariantId);
        Assert.Equal("ABC123", result.ArticleNumber);
        Assert.Equal(120.00m, result.Price);
        Assert.Equal(80.00m, result.DiscountPrice);
        Assert.Equal(20.00m, result.DicountPercentage);
        Assert.Equal("USD", result.CurrencyCode);
    }

    [Fact]
    public async Task UpdateAsync_Should_Not_Update_Nonexistent_ProductPriceEntity()
    {
        // Arrange
        var repository = new PriceRepository(_context);
        var entity = new ProductPriceEntity
        {
            ProductVariantId = 1,
            ArticleNumber = "ABC123",
            Price = 100.00m,
            DiscountPrice = 80.00m,
            DicountPercentage = 20.00m,
            CurrencyCode = "USD"
        };

        entity.Price = 120.00m;

        // Act
        var result = await repository.UpdateAsync(p => p.Id == entity.Id, entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_ProductPriceEntity_Successfully()
    {
        // Arrange
        var repository = new PriceRepository(_context);
        var entity = new ProductPriceEntity
        {
            ProductVariantId = 1,
            ArticleNumber = "ABC123",
            Price = 100.00m,
            DiscountPrice = 80.00m,
            DicountPercentage = 20.00m,
            CurrencyCode = "USD"
        };
        await repository.AddAsync(entity);

        // Act
        var result = await repository.RemoveAsync(p => p.Id == entity.Id);

        // Assert
        Assert.True(result);

        var removedProductPrice = await repository.GetOneAsync(p => p.Id == entity.Id);
        Assert.Null(removedProductPrice);
    }

    [Fact]
    public async Task RemoveAsync_Should_Not_Remove_Nonexistent_ProductPriceEntity()
    {
        // Arrange
        var repository = new PriceRepository(_context);
        var entity = new ProductPriceEntity
        {
            ProductVariantId = 1,
            ArticleNumber = "ABC123",
            Price = 100.00m,
            DiscountPrice = 80.00m,
            DicountPercentage = 20.00m,
            CurrencyCode = "USD"
        };

        // Act
        var result = await repository.RemoveAsync(p => p.Id == entity.Id);

        // Assert
        Assert.False(result);
    }
}
