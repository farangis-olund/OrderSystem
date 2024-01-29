using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests;

public class OrderDetailRepositoryTests
{
    private readonly CustomerOrderContext _context =
        new(new DbContextOptionsBuilder<CustomerOrderContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);
    private readonly ProductDataContext _productContext =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

    [Fact]
    public async Task AddAsync_Should_Add_OrderDetailEntity_Successfully()
    {
        // Arrange
        var repository = new OrderDetailRepository(_context);

        var orderDetailEntity = new OrderDetailEntity
        {
            CustomerOrderId = 1, 
            ProductVariantId = 1, 
            Quantity = 5
        };

        // Act 
        var result = await repository.AddAsync(orderDetailEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderDetailEntity.CustomerOrderId, result.CustomerOrderId);
    }
       
    [Fact]
    public async Task UpdateAsync_Should_Update_OrderDetailEntity_Successfully()
    {
        // Arrange
        var repository = new OrderDetailRepository(_context);

        var orderDetailEntity = new OrderDetailEntity
        {
            CustomerOrderId = 1,  // Assuming you have a CustomerOrder with Id 1
            ProductVariantId = 1, // Assuming you have a ProductVariant with Id 1
            Quantity = 5
        };

        await repository.AddAsync(orderDetailEntity);

        orderDetailEntity.Quantity = 10;

        // Act
        var result = await repository.UpdateAsync(o => o.OrderDetailId == orderDetailEntity.OrderDetailId, orderDetailEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Quantity);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_OrderDetailEntity_Successfully()
    {
        // Arrange
        var repository = new OrderDetailRepository(_context);

        var orderDetailEntity = new OrderDetailEntity
        {
            CustomerOrderId = 1,  // Assuming you have a CustomerOrder with Id 1
            ProductVariantId = 1, // Assuming you have a ProductVariant with Id 1
            Quantity = 5
        };

        await repository.AddAsync(orderDetailEntity);

        // Act
        var result = await repository.RemoveAsync(o => o.OrderDetailId == orderDetailEntity.OrderDetailId);

        // Assert
        Assert.True(result);

        var removedOrderDetail = await repository.GetOneAsync(o => o.OrderDetailId == orderDetailEntity.OrderDetailId);
        Assert.Null(removedOrderDetail);
    }

    [Fact]
    public async Task GetOneAsync_Should_Return_OrderDetailEntity()
    {
        // Arrange
        var repository = new OrderDetailRepository(_context);

        // Seed data
        var orderDetailEntity = new OrderDetailEntity
        {
            CustomerOrderId = 1, 
            ProductVariantId = 1, 
            Quantity = 5
        };

        _context.OrderDetails.Add(orderDetailEntity);
        _context.SaveChanges();

        // Act
        var result = await repository.GetOneAsync(o => o.OrderDetailId == orderDetailEntity.OrderDetailId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Quantity);
    }
}
