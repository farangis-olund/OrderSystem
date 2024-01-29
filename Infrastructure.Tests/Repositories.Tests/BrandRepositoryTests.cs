using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests;

 public class BrandRepositoryTests
{
    private readonly ProductDataContext _context =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);
        
   
    [Fact]
    public async Task AddAsync_Should_Add_BrandEntity_Successfully()
    {
        // Arrange
        var repository = new BrandRepository(_context);

        var entity = new BrandEntity { BrandName = "BrandTest" };

        // Act 
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.BrandName, result.BrandName);
    }

    [Fact]
    public async Task AddAsync_Should_Not_Add_Duplicate_BrandEntity()
    {
        // Arrange
        var repository = new BrandRepository(_context);

        var entity = new BrandEntity { BrandName = "BrandTest" };

        await repository.AddAsync(entity);

        // Act
        var result = await repository.AddAsync(entity);

        // Assert
        Assert.Null(result); 
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_BrandEntities()
    {
        // Arrange
        var repository = new BrandRepository(_context);
        var testData = new List<BrandEntity>
        {
            new() {  BrandName ="Test1" },
            new() { BrandName ="Test2" }
        };

        _context.BrandEntities.AddRange(testData);
        _context.SaveChanges();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_BrandEntity_Successfully()
    {
        // Arrange
        var repository = new BrandRepository(_context);
        var entity = new BrandEntity { BrandName = "BrandTest" };
        
        await repository.AddAsync(entity);
        entity.BrandName = "UpdatedBrandName";

        // Act
        var result = await repository.UpdateAsync(b => b.Id == entity.Id, entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UpdatedBrandName", result.BrandName);
    }

    [Fact]
    public async Task UpdateAsync_Should_Not_Update_Nonexistent_BrandEntity()
    {
        // Arrange
        var repository = new BrandRepository(_context);
        var entity = new BrandEntity { BrandName = "BrandTest" };

        entity.BrandName = "UpdatedBrandName";

        // Act
        var result = await repository.UpdateAsync(b => b.Id == entity.Id, entity);

        // Assert
        Assert.Null(result); 
    }


    [Fact]
    public async Task RemoveAsync_Should_Remove_BrandEntity_Successfully()
    {
        // Arrange
        var repository = new BrandRepository(_context);
        var entity = new BrandEntity { BrandName = "BrandTest" };
        await repository.AddAsync(entity);

        // Act
        var result = await repository.RemoveAsync(b => b.Id == entity.Id);

        // Assert
        Assert.True(result);

        var removedBrand = await repository.GetOneAsync(b => b.Id == entity.Id);
        Assert.Null(removedBrand);
    }

    [Fact]
    public async Task RemoveAsync_Should_Not_Remove_Nonexistent_BrandEntity()
    {
        // Arrange
        var repository = new BrandRepository(_context);
        var entity = new BrandEntity { BrandName = "BrandTest" };

        // Act
        var result = await repository.RemoveAsync(b => b.Id == entity.Id);

        // Assert
        Assert.False(result); 
    }

}





