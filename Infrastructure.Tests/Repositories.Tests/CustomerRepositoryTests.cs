using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Repositories.Tests
{
    public class CustomerRepositoryTests
    {
        private readonly CustomerOrderContext _context =
            new(new DbContextOptionsBuilder<CustomerOrderContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);

        [Fact]
        public async Task AddAsync_Should_Add_CustomerEntity_Successfully()
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            var customerEntity = new CustomerEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890"
            };

            // Act 
            var result = await repository.AddAsync(customerEntity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerEntity.FirstName, result.FirstName);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_CustomerEntities()
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            // Seed data
            var testData = new List<CustomerEntity>
            {
                new CustomerEntity { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" },
                new CustomerEntity { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", PhoneNumber = "9876543210" }
            };

            _context.Customers.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_CustomerEntity_Successfully()
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            var customerEntity = new CustomerEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890"
            };

            await repository.AddAsync(customerEntity);

            customerEntity.FirstName = "UpdatedJohn";

            // Act
            var result = await repository.UpdateAsync(c => c.Id == customerEntity.Id, customerEntity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedJohn", result.FirstName);
        }

        [Fact]
        public async Task RemoveAsync_Should_Remove_CustomerEntity_Successfully()
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            var customerEntity = new CustomerEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890"
            };

            await repository.AddAsync(customerEntity);

            // Act
            var result = await repository.RemoveAsync(c => c.Id == customerEntity.Id);

            // Assert
            Assert.True(result);

            var removedCustomer = await repository.GetOneAsync(c => c.Id == customerEntity.Id);
            Assert.Null(removedCustomer);
        }

        [Fact]
        public async Task GetOneAsync_Should_Return_CustomerEntity()
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            // Seed data
            var customerEntity = new CustomerEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890"
            };

            _context.Customers.Add(customerEntity);
            _context.SaveChanges();

            // Act
            var result = await repository.GetOneAsync(c => c.Id == customerEntity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
        }
    }
}
