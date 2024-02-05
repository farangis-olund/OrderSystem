using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;


namespace Infrastructure.Tests.Services.Tests
{
    public class OrderDetailServiceTests
    {
        private readonly InMemoryOrderDetailRepository _inMemoryOrderDetailRepository;
        private readonly OrderDetailService _orderDetailService;
       
         private readonly ILogger<OrderDetailService> _logger;

        public OrderDetailServiceTests()
        {
            var context = new CustomerOrderContext(new DbContextOptionsBuilder<CustomerOrderContext>()
              .UseInMemoryDatabase($"{Guid.NewGuid()}")
              .Options);
           
            _inMemoryOrderDetailRepository = new InMemoryOrderDetailRepository(context);
                       
            _logger = new Mock<ILogger<OrderDetailService>>().Object;

            _orderDetailService = new OrderDetailService(_inMemoryOrderDetailRepository, _logger);
        }

        [Fact]
        public async Task AddOrderDetailAsync_ShouldAddOrderDetail_WhenOrderDetailDoesNotExist()
        {
            // Arrange
            var orderDetailDto = new OrderDetail
            {
                CustomerOrderId = 1,
                ProductVariantId = 1,
                Quantity = 5
            };

            // Act
            var result = await _orderDetailService.AddOrderDetailAsync(orderDetailDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Quantity);
        }

        [Fact]
        public async Task GetOrderDetailAsync_ShouldReturnOrderDetail_WhenOrderDetailExists()
        {
            // Arrange
            var existingOrderDetail = await _inMemoryOrderDetailRepository.AddAsync(
                    new OrderDetailEntity { OrderDetailId = 1, CustomerOrderId = 1, ProductVariantId = 1, Quantity = 10 });

            // Act
            var result = await _orderDetailService.GetOrderDetailAsync(existingOrderDetail);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Quantity);
        }

        
        [Fact]
        public async Task GetAllOrderDetailsAsync_ShouldReturnAllOrderDetails_WhenOrderDetailsExist()
        {
            // Arrange
            var testData = new List<OrderDetailEntity>
            {
                new OrderDetailEntity { OrderDetailId = 1, Quantity = 20 },
                new OrderDetailEntity { OrderDetailId = 2, Quantity = 25 }
            };

            foreach (var orderDetail in testData)
            {
                await _inMemoryOrderDetailRepository.AddAsync(orderDetail);
            }

            // Act
            var result = await _orderDetailService.GetAllOrderDetailsAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(testData.Count, result.Count());
        }

        [Fact]
        public async Task UpdateOrderDetailAsync_ShouldUpdateOrderDetail_WhenOrderDetailExists()
        {
            // Arrange
            await _inMemoryOrderDetailRepository.AddAsync(
                  new OrderDetailEntity { OrderDetailId = 1, CustomerOrderId = 1, ProductVariantId = 1, Quantity = 10 });


            var updatedOrderDetail = new OrderDetailEntity { OrderDetailId = 1, CustomerOrderId = 1, ProductVariantId = 1, Quantity = 35 };

            // Act
            var result = await _orderDetailService.UpdateOrderDetailAsync(updatedOrderDetail);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(35, result.Quantity);
        }

        [Fact]
        public async Task DeleteOrderDetailAsync_ShouldDeleteOrderDetail_WhenOrderDetailExists()
        {
            // Arrange
            var existingOrderDetail = new OrderDetailEntity 
            {   OrderDetailId = 1, 
                CustomerOrderId = 1, 
                ProductVariantId =1, 
                Quantity = 40 };
            
            await _inMemoryOrderDetailRepository.AddAsync(existingOrderDetail);

            var orderDetailDto = new OrderDetail 
            {   OrderDetailId = existingOrderDetail.OrderDetailId, 
                CustomerOrderId =existingOrderDetail.CustomerOrderId, 
                ProductVariantId =existingOrderDetail.ProductVariantId, 
                Quantity = existingOrderDetail.Quantity };

            // Act
            var result = await _orderDetailService.DeleteOrderDetailAsync(orderDetailDto);

            // Assert
            Assert.True(result);
           
        }
       
    }

    public class InMemoryOrderDetailRepository(CustomerOrderContext context) : OrderDetailRepository(context)
    {
        private readonly List<OrderDetailEntity> _orderDetails = new List<OrderDetailEntity>();

        public override Task<OrderDetailEntity> GetOneAsync(Expression<Func<OrderDetailEntity, bool>> predicate)
        {
            return Task.FromResult(_orderDetails.AsQueryable().FirstOrDefault(predicate.Compile())!);
        }

        public override Task<IEnumerable<OrderDetailEntity>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<OrderDetailEntity>>(_orderDetails);
        }

        public override Task<OrderDetailEntity> AddAsync(OrderDetailEntity entity)
        {
            _orderDetails.Add(entity);
            return Task.FromResult(entity);
        }

        public override Task<OrderDetailEntity> UpdateAsync(Expression<Func<OrderDetailEntity, bool>> predicate, OrderDetailEntity entity)
        {
            var existingOrderDetail = _orderDetails.AsQueryable().FirstOrDefault(predicate.Compile());
            if (existingOrderDetail != null)
            {
                existingOrderDetail.Quantity = entity.Quantity;
                existingOrderDetail.CustomerOrderId = entity.CustomerOrderId;
                existingOrderDetail.ProductVariantId = entity.ProductVariantId;

                return Task.FromResult(existingOrderDetail);
            }

            return Task.FromResult<OrderDetailEntity>(null!);
        }

        public override Task<bool> RemoveAsync(Expression<Func<OrderDetailEntity, bool>> predicate)
        {
            var orderDetailToRemove = _orderDetails.AsQueryable().FirstOrDefault(predicate.Compile());
            if (orderDetailToRemove != null)
            {
                _orderDetails.Remove(orderDetailToRemove);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }

}
