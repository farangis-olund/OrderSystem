using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests;

public class CurrencyRepositoryTests
{
    private readonly ProductDataContext _context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task AddAsync_Should_Add_CurrencyEntity_Successfully()
    {
        // Arrange
        var repository = new CurrencyRepository(_context);

        var entity = new CurrencyEntity { Code = "Sek", CurrencyName = "Swedish Krona" };

        // Act 
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sek", result.Code);
        Assert.Equal("Swedish Krona", result.CurrencyName);
    }

    [Fact]
    public async Task AddAsync_Should_Not_Add_Duplicate_CurrencyEntity()
    {
        // Arrange
        var repository = new CurrencyRepository(_context);

        var entity = new CurrencyEntity { Code = "Sek", CurrencyName = "Swedish Krona" };

        await repository.AddAsync(entity);

        // Act
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_CurrencyEntities()
    {
        // Arrange
        var repository = new CurrencyRepository(_context);
        var testData = new List<CurrencyEntity>
        {
            new CurrencyEntity { Code = "USD", CurrencyName = "US Dollar" },
            new CurrencyEntity { Code = "EUR", CurrencyName = "Euro" }
        };

        _context.CurrencyEntities.AddRange(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_CurrencyEntity_Successfully()
    {
        // Arrange
        var repository = new CurrencyRepository(_context);
        var entity = new CurrencyEntity { Code = "Sek", CurrencyName = "Swedish Krona" };

        await repository.AddAsync(entity);
        entity.CurrencyName = "Updated Swedish Krona";

        // Act
        var result = await repository.UpdateAsync(c => c.Code == entity.Code, entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sek", result.Code);
        Assert.Equal("Updated Swedish Krona", result.CurrencyName);
    }

    [Fact]
    public async Task UpdateAsync_Should_Not_Update_Nonexistent_CurrencyEntity()
    {
        // Arrange
        var repository = new CurrencyRepository(_context);
        var entity = new CurrencyEntity { Code = "Sek", CurrencyName = "Swedish Krona" };

        entity.CurrencyName = "Updated Swedish Krona";

        // Act
        var result = await repository.UpdateAsync(c => c.Code == entity.Code, entity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_CurrencyEntity_Successfully()
    {
        // Arrange
        var repository = new CurrencyRepository(_context);
        var entity = new CurrencyEntity { Code = "Sek", CurrencyName = "Swedish Krona" };
        await repository.AddAsync(entity);

        // Act
        var result = await repository.RemoveAsync(c => c.Code == entity.Code);

        // Assert
        Assert.True(result);

        var removedCurrency = await repository.GetOneAsync(c => c.Code == entity.Code);
        Assert.Null(removedCurrency);
    }

    [Fact]
    public async Task RemoveAsync_Should_Not_Remove_Nonexistent_CurrencyEntity()
    {
        // Arrange
        var repository = new CurrencyRepository(_context);
        var entity = new CurrencyEntity { Code = "Sek", CurrencyName = "Swedish Krona" };

        // Act
        var result = await repository.RemoveAsync(c => c.Code == entity.Code);

        // Assert
        Assert.False(result);
    }
}
