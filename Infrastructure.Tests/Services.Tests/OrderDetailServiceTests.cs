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
using static Infrastructure.Tests.Services.Tests.ProductServiceTests;

namespace Infrastructure.Tests.Services.Tests
{
    public class OrderDetailServiceTests
    {
        private readonly InMemoryOrderDetailRepository _inMemoryOrderDetailRepository;
        private readonly InMemoryCustomerOrderRepository _inMemoryCustomerOrderRepository;
        private readonly InMemoryProductVariantRepository _inMemoryProductVariantRepository;
        private readonly OrderDetailService _orderDetailService;
        private readonly CustomerService _customerService;
        private readonly InMemoryCustomerRepository _inMemoryCustomerRepository;
        private readonly InMemoryColorRepository _inMemoryColorRepository;
        private readonly InMemorySizeRepository _inMemorySizeRepository;
        private readonly InMemoryProductRepository _inMemoryProductRepository;
        private readonly InMemoryBrandRepository _inMemoryBrandRepository;
        private readonly InMemoryCategoryRepository _inMemoryCategoryRepository;
        private readonly ILogger<OrderDetailService> _logger;

        public OrderDetailServiceTests()
        {
            var context = new CustomerOrderContext(new DbContextOptionsBuilder<CustomerOrderContext>()
              .UseInMemoryDatabase($"{Guid.NewGuid()}")
              .Options);

            var contextProduct = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
              .UseInMemoryDatabase($"{Guid.NewGuid()}")
              .Options);

            _inMemoryOrderDetailRepository = new InMemoryOrderDetailRepository(context);
            
            _inMemoryCustomerOrderRepository = new InMemoryCustomerOrderRepository(context);
            
            _inMemoryProductVariantRepository = new InMemoryProductVariantRepository(contextProduct);
            
            _inMemoryCustomerRepository = new InMemoryCustomerRepository(context);

            _inMemoryProductRepository = new InMemoryProductRepository(contextProduct);

            _inMemoryBrandRepository = new InMemoryBrandRepository(contextProduct);

            _inMemoryCategoryRepository = new InMemoryCategoryRepository(contextProduct);

            _inMemoryColorRepository = new InMemoryColorRepository(contextProduct);

            _inMemorySizeRepository = new InMemorySizeRepository(contextProduct);

            _customerService = new CustomerService(_inMemoryCustomerRepository, new Mock<ILogger<CustomerService>>().Object);

            var colorService = new ColorService(_inMemoryColorRepository, new Mock<ILogger<ColorService>>().Object);

            var sizeService = new SizeService(_inMemorySizeRepository, new Mock<ILogger<SizeService>>().Object);
            
            var brandService = new BrandService(_inMemoryBrandRepository, new Mock<ILogger<BrandService>>().Object);

            var categoryService = new CategoryService(_inMemoryCategoryRepository, new Mock<ILogger<CategoryService>>().Object);

            var productService = new ProductService (_inMemoryProductRepository, brandService, categoryService, new Mock<ILogger<ProductService>>().Object);


            var customerOrderService = new CustomerOrderService(_inMemoryCustomerOrderRepository, _customerService, new Mock<ILogger<CustomerOrderService>>().Object);
            
            var productVariantService = new ProductVariantService(_inMemoryProductVariantRepository, productService, sizeService, colorService, new Mock<ILogger<ProductVariantService>>().Object);

            _logger = new Mock<ILogger<OrderDetailService>>().Object;

            _orderDetailService = new OrderDetailService(_inMemoryOrderDetailRepository, customerOrderService, productVariantService, _logger);
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
            var existingOrderDetail = await _inMemoryOrderDetailRepository.AddAsync(
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

    public class InMemoryOrderDetailRepository : OrderDetailRepository
    {
        private readonly List<OrderDetailEntity> _orderDetails;

        public InMemoryOrderDetailRepository(CustomerOrderContext context) : base(context)
        {
            _orderDetails = new List<OrderDetailEntity>();
        }

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

    public class InMemoryProductVariantRepository : ProductVariantRepository
    {
        private readonly List<ProductVariantEntity> _productVariants;

        public InMemoryProductVariantRepository(ProductDataContext context) : base(context)
        {
            _productVariants = new List<ProductVariantEntity>();
        }

        public override Task<ProductVariantEntity> GetOneAsync(Expression<Func<ProductVariantEntity, bool>> predicate)
        {
            return Task.FromResult(_productVariants.AsQueryable().FirstOrDefault(predicate.Compile())!);
        }

        public override Task<IEnumerable<ProductVariantEntity>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<ProductVariantEntity>>(_productVariants);
        }

        public override Task<ProductVariantEntity> AddAsync(ProductVariantEntity entity)
        {
            _productVariants.Add(entity);
            return Task.FromResult(entity);
        }

        public override Task<ProductVariantEntity> UpdateAsync(Expression<Func<ProductVariantEntity, bool>> predicate, ProductVariantEntity entity)
        {
            var existingProductVariant = _productVariants.AsQueryable().FirstOrDefault(predicate.Compile());
            if (existingProductVariant != null)
            {
                existingProductVariant.ArticleNumber = entity.ArticleNumber;
                existingProductVariant.SizeId = entity.SizeId;
                existingProductVariant.ColorId = entity.ColorId;
                existingProductVariant.Quantity = entity.Quantity;

                return Task.FromResult(existingProductVariant);
            }

            return Task.FromResult<ProductVariantEntity>(null!);
        }

        public override Task<bool> RemoveAsync(Expression<Func<ProductVariantEntity, bool>> predicate)
        {
            var productVariantToRemove = _productVariants.AsQueryable().FirstOrDefault(predicate.Compile());
            if (productVariantToRemove != null)
            {
                _productVariants.Remove(productVariantToRemove);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
