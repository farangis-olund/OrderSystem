using System.Linq.Expressions;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;


namespace Infrastructure.Tests.Services.Tests
{
    public class CustomerServiceTests
    {
        private readonly InMemoryCustomerRepository _inMemoryCustomerRepository;
        private readonly Mock<ILogger<CustomerService>> _mockLogger;

        public CustomerServiceTests()
        {
            var context = new CustomerOrderContext(new DbContextOptionsBuilder<CustomerOrderContext>()
           .UseInMemoryDatabase($"{Guid.NewGuid()}")
           .Options);
            _inMemoryCustomerRepository = new InMemoryCustomerRepository(context);
            _mockLogger = new Mock<ILogger<CustomerService>>();
        }

        [Fact]
        public async Task AddCustomerAsync_ShouldAddCustomer_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerService = new CustomerService(_inMemoryCustomerRepository, _mockLogger.Object);
            var customer = new CustomerEntity { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "123456789" };

            // Act
            var result = await customerService.AddCustomerAsync(customer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
        }

        [Fact]
        public async Task AddCustomerAsync_ShouldNotAddCustomer_WhenCustomerExists()
        {
            // Arrange
            var existingCustomer = new CustomerEntity { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567" };
            await _inMemoryCustomerRepository.AddAsync(existingCustomer);

            var customerService = new CustomerService(_inMemoryCustomerRepository, _mockLogger.Object);
            var customer = new CustomerEntity { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "9876543" };

            // Act
            var result = await customerService.AddCustomerAsync(customer);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCustomerAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            var existingCustomer = new CustomerEntity { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "123456789" };
            await _inMemoryCustomerRepository.AddAsync(existingCustomer);

            var customerService = new CustomerService(_inMemoryCustomerRepository, _mockLogger.Object);

            // Act
            var result = await customerService.GetCustomerAsync("john.doe@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
        }

        [Fact]
        public async Task GetCustomerAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerService = new CustomerService(_inMemoryCustomerRepository, _mockLogger.Object);

            // Act
            var result = await customerService.GetCustomerAsync("nonexistent@example.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldUpdateCustomer_WhenCustomerExists()
        {
            // Arrange
            var existingCustomer = new CustomerEntity { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "123456789" };
            await _inMemoryCustomerRepository.AddAsync(existingCustomer);

            var customerService = new CustomerService(_inMemoryCustomerRepository, _mockLogger.Object);
            var updatedCustomer = new CustomerEntity { FirstName = "Updated", LastName = "Customer", Email = "john.doe@example.com", PhoneNumber = "987654321" };

            // Act
            var result = await customerService.UpdateCustomerAsync(updatedCustomer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated", result.FirstName);
            Assert.Equal("Customer", result.LastName);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerService = new CustomerService(_inMemoryCustomerRepository, _mockLogger.Object);
            var nonExistentCustomer = new CustomerEntity { FirstName = "Non", LastName = "Existent", Email = "nonexistent@example.com", PhoneNumber = "111222333" };

            // Act
            var result = await customerService.UpdateCustomerAsync(nonExistentCustomer);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldDeleteCustomer_WhenCustomerExists()
        {
            // Arrange
            var existingCustomer = new CustomerEntity { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234" };
            await _inMemoryCustomerRepository.AddAsync(existingCustomer);

            var customerService = new CustomerService(_inMemoryCustomerRepository, _mockLogger.Object);

            // Act
            var result = await customerService.DeleteCustomerAsync("john.doe@example.com");

            // Assert
            Assert.True(result, "Expected the customer to be deleted");
           

        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldReturnFalse_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerService = new CustomerService(_inMemoryCustomerRepository, _mockLogger.Object);

            // Act
            var result = await customerService.DeleteCustomerAsync("nonexistent@example.com");

            // Assert
            Assert.False(result);
        }

        public class InMemoryCustomerRepository : CustomerRepository
        {
            private readonly List<CustomerEntity> _customers;

            public InMemoryCustomerRepository(CustomerOrderContext context) : base(context)
            {
                _customers = new List<CustomerEntity>();
            }

            public override Task<CustomerEntity> GetOneAsync(Expression<Func<CustomerEntity, bool>> predicate)
            {
                return Task.FromResult(_customers.AsQueryable().FirstOrDefault(predicate.Compile())!);
            }

            public override Task<IEnumerable<CustomerEntity>> GetAllAsync()
            {
                return Task.FromResult<IEnumerable<CustomerEntity>>(_customers);
            }

            public override Task<CustomerEntity> AddAsync(CustomerEntity entity)
            {
                _customers.Add(entity);
                return Task.FromResult(entity);
            }

            public override Task<CustomerEntity> UpdateAsync(Expression<Func<CustomerEntity, bool>> predicate, CustomerEntity entity)
            {
                var existingCustomer = _customers.AsQueryable().FirstOrDefault(predicate.Compile());
                if (existingCustomer != null)
                {
                    existingCustomer.FirstName = entity.FirstName;
                    existingCustomer.LastName = entity.LastName;
                    existingCustomer.Email = entity.Email;
                    existingCustomer.PhoneNumber = entity.PhoneNumber;

                    return Task.FromResult(existingCustomer);
                }

                return Task.FromResult<CustomerEntity>(null!);
            }

            public override Task<bool> RemoveAsync(Expression<Func<CustomerEntity, bool>> predicate)
            {
                var customerToRemove = _customers.AsQueryable().FirstOrDefault(predicate.Compile());
                if (customerToRemove != null)
                {
                    _customers.Remove(customerToRemove);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
        }
    }
}
