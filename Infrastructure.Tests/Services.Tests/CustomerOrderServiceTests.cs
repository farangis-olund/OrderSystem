using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using static Infrastructure.Tests.Services.Tests.CustomerServiceTests;


namespace Infrastructure.Tests.Services.Tests
{
    public class CustomerOrderServiceTests
    {
        private readonly InMemoryCustomerOrderRepository _inMemoryCustomerOrderRepository;
        private readonly InMemoryCustomerRepository _inMemoryCustomerRepository;
        private readonly CustomerService _customerService;
        private readonly ILogger<CustomerOrderService> _logger;

        public CustomerOrderServiceTests()
        {
            var context = new CustomerOrderContext(new DbContextOptionsBuilder<CustomerOrderContext>()
              .UseInMemoryDatabase($"{Guid.NewGuid()}")
              .Options);
            _inMemoryCustomerOrderRepository = new InMemoryCustomerOrderRepository(context);
            _inMemoryCustomerRepository = new InMemoryCustomerRepository(context);
            _customerService = new CustomerService(_inMemoryCustomerRepository, new Mock<ILogger<CustomerService>>().Object);
            _logger = new Mock<ILogger<CustomerOrderService>>().Object;
        }

        [Fact]
        public async Task AddCustomerOrderAsync_ShouldAddCustomerOrder_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerOrderService = new CustomerOrderService(_inMemoryCustomerOrderRepository, _customerService, _logger);
            var customerOrderDto = new CustomerOrder
            {
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                CustomerEmail = "john.doe@example.com",
                CustomerPhoneNumber = "1234567",
                TotalAmount = 100,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            // Act
            var result = await customerOrderService.AddCustomerOrderAsync(customerOrderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.TotalAmount);
        }

        [Fact]
        public async Task GetCustomerOrderAsync_ShouldReturnCustomerOrder_WhenCustomerOrderExists()
        {
            // Arrange
            var existingCustomerOrder = new CustomerOrderEntity { Id = 1, TotalAmount = 50, Date = DateOnly.FromDateTime(DateTime.Now) };
            await _inMemoryCustomerOrderRepository.AddAsync(existingCustomerOrder);

            var customerOrderService = new CustomerOrderService(_inMemoryCustomerOrderRepository, _customerService, _logger);
            var customerOrderDto = new CustomerOrder { CustomerOrderId = 1 };

            // Act
            var result = await customerOrderService.GetCustomerOrderAsync(customerOrderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(50, result.TotalAmount);
        }

        [Fact]
        public async Task GetAllCustomerOrdersAsync_ShouldReturnAllCustomerOrders_WhenCustomerOrdersExist()
        {
            // Arrange
            var testData = new List<CustomerOrderEntity>
            {
                new CustomerOrderEntity { Id = 1, TotalAmount = 50, Date = DateOnly.FromDateTime(DateTime.Now) },
                new CustomerOrderEntity { Id = 2, TotalAmount = 75, Date = DateOnly.FromDateTime(DateTime.Now).AddDays(-1) }
            };

            foreach (var customerOrder in testData)
            {
                await _inMemoryCustomerOrderRepository.AddAsync(customerOrder);
            }

            var customerOrderService = new CustomerOrderService(_inMemoryCustomerOrderRepository, _customerService, _logger);

            // Act
            var result = await customerOrderService.GetAllCustomerOrdersAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(testData.Count, result.Count());
        }

        [Fact]
        public async Task UpdateCustomerOrderAsync_ShouldUpdateCustomerOrder_WhenCustomerOrderExists()
        {
            // Arrange
            var existingCustomerOrder = new CustomerOrderEntity { Id = 1, TotalAmount = 50, Date = DateOnly.FromDateTime(DateTime.Now) };
            await _inMemoryCustomerOrderRepository.AddAsync(existingCustomerOrder);

            var updatedCustomerOrder = new CustomerOrderEntity { Id = 1, TotalAmount = 70, Date = DateOnly.FromDateTime(DateTime.Now) };

            var customerOrderService = new CustomerOrderService(_inMemoryCustomerOrderRepository, _customerService, _logger);

            // Act
            var result = await customerOrderService.UpdateCustomerOrderAsync(updatedCustomerOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(70, result.TotalAmount);
        }

        [Fact]
        public async Task DeleteCustomerOrderAsync_ShouldDeleteCustomerOrder_WhenCustomerOrderExists()
        {
            // Arrange
            var existingCustomerOrder = new CustomerOrderEntity { Id =1, TotalAmount = 50, Date = DateOnly.FromDateTime(DateTime.Now), CustomerId = Guid.NewGuid() };
            await _inMemoryCustomerOrderRepository.AddAsync(existingCustomerOrder);

            var customerOrderService = new CustomerOrderService(_inMemoryCustomerOrderRepository, _customerService, _logger);
            var customerOrderDto = new CustomerOrder { CustomerOrderId = 1, CustomerId = existingCustomerOrder.CustomerId, TotalAmount = existingCustomerOrder.TotalAmount, Date = existingCustomerOrder.Date  };

            // Act
            var result = await customerOrderService.DeleteCustomerOrderAsync(customerOrderDto);

            // Assert
            Assert.True(result);
         
        }

        
    }

    public class InMemoryCustomerOrderRepository : CustomerOrderRepository
    {
        private readonly List<CustomerOrderEntity> _customerOrders;

        public InMemoryCustomerOrderRepository(CustomerOrderContext context) : base(context)
        {
            
            _customerOrders = new List<CustomerOrderEntity>();
        }

        public override Task<CustomerOrderEntity> GetOneAsync(Expression<Func<CustomerOrderEntity, bool>> predicate)
        {
            return Task.FromResult(_customerOrders.AsQueryable().FirstOrDefault(predicate.Compile())!);
        }

        public override Task<IEnumerable<CustomerOrderEntity>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<CustomerOrderEntity>>(_customerOrders);
        }

        public override Task<CustomerOrderEntity> AddAsync(CustomerOrderEntity entity)
        {
            _customerOrders.Add(entity);
            return Task.FromResult(entity);
        }

        public override Task<CustomerOrderEntity> UpdateAsync(Expression<Func<CustomerOrderEntity, bool>> predicate, CustomerOrderEntity entity)
        {
            var existingCustomerOrder = _customerOrders.AsQueryable().FirstOrDefault(predicate.Compile());
            if (existingCustomerOrder != null)
            {
                existingCustomerOrder.TotalAmount = entity.TotalAmount;
                existingCustomerOrder.Date = entity.Date;
                existingCustomerOrder.CustomerId = entity.CustomerId;

                return Task.FromResult(existingCustomerOrder);
            }

            return Task.FromResult<CustomerOrderEntity>(null!);
        }

        public override Task<bool> RemoveAsync(Expression<Func<CustomerOrderEntity, bool>> predicate)
        {
            var customerOrderToRemove = _customerOrders.AsQueryable().FirstOrDefault(predicate.Compile());
            if (customerOrderToRemove != null)
            {
                _customerOrders.Remove(customerOrderToRemove);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
