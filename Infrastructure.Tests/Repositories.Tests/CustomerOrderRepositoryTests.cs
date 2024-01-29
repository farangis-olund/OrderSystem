using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests
{
    public class CustomerOrderRepositoryTests
    {
        private readonly CustomerOrderContext _context =
            new(new DbContextOptionsBuilder<CustomerOrderContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);

        [Fact]
        public async Task AddAsync_Should_Add_CustomerOrderEntity_Successfully()
        {
            // Arrange
            var repository = new CustomerOrderRepository(_context);

            var customerOrderEntity = new CustomerOrderEntity
            {
                TotalAmount = 100,
                Date = new DateOnly(2022, 1, 1),
                CustomerId = Guid.NewGuid()
            };

            // Act 
            var result = await repository.AddAsync(customerOrderEntity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerOrderEntity.TotalAmount, result.TotalAmount);
        }
               

        [Fact]
        public async Task UpdateAsync_Should_Update_CustomerOrderEntity_Successfully()
        {
            // Arrange
            var repository = new CustomerOrderRepository(_context);

            var customerOrderEntity = new CustomerOrderEntity
            {
                TotalAmount = 100,
                Date = new DateOnly(2022, 1, 1),
                CustomerId = Guid.NewGuid()
            };

            await repository.AddAsync(customerOrderEntity);

            customerOrderEntity.TotalAmount = 150;

            // Act
            var result = await repository.UpdateAsync(c => c.Id == customerOrderEntity.Id, customerOrderEntity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(150, result.TotalAmount);
        }

        [Fact]
        public async Task RemoveAsync_Should_Remove_CustomerOrderEntity_Successfully()
        {
            // Arrange
            var repository = new CustomerOrderRepository(_context);

            var customerOrderEntity = new CustomerOrderEntity
            {
                TotalAmount = 100,
                Date = new DateOnly(2022, 1, 1),
                CustomerId = Guid.NewGuid()
            };

            await repository.AddAsync(customerOrderEntity);

            // Act
            var result = await repository.RemoveAsync(c => c.Id == customerOrderEntity.Id);

            // Assert
            Assert.True(result);

            var removedCustomerOrder = await repository.GetOneAsync(c => c.Id == customerOrderEntity.Id);
            Assert.Null(removedCustomerOrder);
        }

        [Fact]
        public async Task GetOneAsync_Should_Return_CustomerOrderEntity()
        {
            // Arrange
            var repository = new CustomerOrderRepository(_context);

            // Seed data
            var customerOrderEntity = new CustomerOrderEntity
            {
                TotalAmount = 100,
                Date = new DateOnly(2022, 1, 1),
                CustomerId = Guid.NewGuid()
            };

            _context.CustomerOrders.Add(customerOrderEntity);
            _context.SaveChanges();

            // Act
            var result = await repository.GetOneAsync(c => c.Id == customerOrderEntity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.TotalAmount);
        }
    }
}
