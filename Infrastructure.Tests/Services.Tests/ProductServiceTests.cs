using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;


namespace Infrastructure.Tests.Services.Tests
{
    public class ProductServiceTests
    {
        private readonly InMemoryProductRepository _inMemoryProductRepository;
        private readonly InMemoryBrandRepository _inMemoryBrandRepository; 
        private readonly InMemoryCategoryRepository _inMemoryCategoryRepository; 
        private readonly Mock<ILogger<ProductService>> _mockLoggerProduct;
        private readonly Mock<ILogger<BrandService>> _mockLoggerBrand;
        private readonly Mock<ILogger<CategoryService>> _mockLoggerCategory;


        public ProductServiceTests()
        {
            var context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _inMemoryProductRepository = new InMemoryProductRepository(context); 
            _inMemoryBrandRepository = new InMemoryBrandRepository(context); 
            _inMemoryCategoryRepository = new InMemoryCategoryRepository(context); 

            _mockLoggerProduct = new Mock<ILogger<ProductService>>();
            _mockLoggerBrand = new Mock<ILogger<BrandService>>();
            _mockLoggerCategory = new Mock<ILogger<CategoryService>>();

        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProduct_WhenProductDoesNotExist()
        {
            // Arrange
            
            var productService = new ProductService(_inMemoryProductRepository, new BrandService(_inMemoryBrandRepository, _mockLoggerBrand.Object), new CategoryService(_inMemoryCategoryRepository, _mockLoggerCategory.Object), _mockLoggerProduct.Object);
            var product = new ProductEntity { ArticleNumber = "123", ProductName = "TestProduct", BrandId = 1, Brand = new BrandEntity {Id =1, BrandName = "TestBrand" }, CategoryId = 1, Category = new CategoryEntity { Id = 1, CategoryName = "TestCategory" } };

            // Act
            var result = await productService.AddProductAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123", result.ArticleNumber);
            Assert.Equal("TestProduct", result.ProductName);
        }

        [Fact]
        public async Task AddProductAsync_ShouldNotAddProduct_WhenProductExists()
        {
            // Arrange
            var productService = new ProductService(_inMemoryProductRepository, new BrandService(_inMemoryBrandRepository, _mockLoggerBrand.Object), new CategoryService(_inMemoryCategoryRepository, _mockLoggerCategory.Object), _mockLoggerProduct.Object);
            var product = new ProductEntity { ArticleNumber = "123", ProductName = "TestProduct", BrandId = 1, Brand = new BrandEntity { Id = 1, BrandName = "TestBrand" }, CategoryId = 1, Category = new CategoryEntity { Id = 1, CategoryName = "TestCategory" } };
            await _inMemoryProductRepository.AddAsync(product);

            // Act
            var result = await productService.AddProductAsync(product);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductByArticleAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productService = new ProductService(_inMemoryProductRepository, new BrandService(_inMemoryBrandRepository, _mockLoggerBrand.Object), new CategoryService(_inMemoryCategoryRepository, _mockLoggerCategory.Object), _mockLoggerProduct.Object);
            var product = new ProductEntity { ArticleNumber = "123", ProductName = "TestProduct", BrandId = 1, CategoryId = 1 };
            await _inMemoryProductRepository.AddAsync(product);

            // Act
            var result = await productService.GetProductByArticleAsync("123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123", result.ArticleNumber);
            Assert.Equal("TestProduct", result.ProductName);
        }

        [Fact]
        public async Task GetProductByArticleAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productService = new ProductService(_inMemoryProductRepository, new BrandService(_inMemoryBrandRepository, _mockLoggerBrand.Object), new CategoryService(_inMemoryCategoryRepository, _mockLoggerCategory.Object), _mockLoggerProduct.Object);

            // Act
            var result = await productService.GetProductByArticleAsync("NonExistentArticle");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var productService = new ProductService(_inMemoryProductRepository, new BrandService(_inMemoryBrandRepository, _mockLoggerBrand.Object), new CategoryService(_inMemoryCategoryRepository, _mockLoggerCategory.Object), _mockLoggerProduct.Object);
            var originalProduct = new ProductEntity { ArticleNumber = "123", ProductName = "TestProduct", BrandId = 1, CategoryId = 1 };
            await _inMemoryProductRepository.AddAsync(originalProduct);

            // Act
            originalProduct.ProductName = "UpdatedProduct";
            var result = await productService.UpdateProductAsync(originalProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123", result.ArticleNumber);
            Assert.Equal("UpdatedProduct", result.ProductName);
        }

        [Fact]
        public async Task DeleteProductByArticleAsync_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var productService = new ProductService(_inMemoryProductRepository, new BrandService(_inMemoryBrandRepository, _mockLoggerBrand.Object), new CategoryService(_inMemoryCategoryRepository, _mockLoggerCategory.Object), _mockLoggerProduct.Object);
            var productEntity = new ProductEntity { ArticleNumber = "123", ProductName = "TestProduct", BrandId = 1, CategoryId = 1 };

             await _inMemoryProductRepository.AddAsync(productEntity);

            // Act
            var result = await productService.DeleteProductByArticleAsync("123");

            // Assert
            Assert.True(result);

            var deletedProduct = await _inMemoryProductRepository.GetOneAsync(p => p.ArticleNumber == "123");
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task GetAllProductAsync_ShouldReturnAllProducts_WhenProductsExist()
        {
            // Arrange
            var productService = new ProductService(_inMemoryProductRepository, new BrandService(_inMemoryBrandRepository, _mockLoggerBrand.Object), new CategoryService(_inMemoryCategoryRepository, _mockLoggerCategory.Object), _mockLoggerProduct.Object);
            var testData = new List<ProductEntity>
            {
                new ProductEntity { ArticleNumber = "123", ProductName = "Product1", BrandId = 1, CategoryId = 1 },
                new ProductEntity { ArticleNumber = "456", ProductName = "Product2", BrandId = 1, CategoryId= 1 }
            };

            foreach (var product in testData)
            {
                await _inMemoryProductRepository.AddAsync(product);
            }

            // Act
            var result = await productService.GetAllProductAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(testData.Count, result.Count());
        }

        public class InMemoryProductRepository : ProductRepository
        {
            private readonly List<ProductEntity> _products;

            public InMemoryProductRepository(ProductDataContext context) : base(context)
            {
                _products = new List<ProductEntity>();
            }

            public override Task<ProductEntity> GetOneAsync(Expression<Func<ProductEntity, bool>> predicate)
            {
                return Task.FromResult(_products.AsQueryable().FirstOrDefault(predicate.Compile())!);
            }

            public override Task<IEnumerable<ProductEntity>> GetAllAsync()
            {
                return Task.FromResult<IEnumerable<ProductEntity>>(_products);
            }

            public override Task<ProductEntity> AddAsync(ProductEntity entity)
            {
                _products.Add(entity);
                return Task.FromResult(entity);
            }

            public override Task<ProductEntity> UpdateAsync(Expression<Func<ProductEntity, bool>> predicate, ProductEntity entity)
            {
                var existingProduct = _products.AsQueryable().FirstOrDefault(predicate.Compile());
                if (existingProduct != null)
                {
                    existingProduct.ArticleNumber = entity.ArticleNumber;
                    existingProduct.ProductName = entity.ProductName;
                    existingProduct.Material = entity.Material;
                    existingProduct.ProductInfo = entity.ProductInfo;
                    existingProduct.BrandId = entity.BrandId;
                    existingProduct.CategoryId = entity.CategoryId;

                    return Task.FromResult(existingProduct);
                }

                return Task.FromResult<ProductEntity>(null!);
            }

            public override Task<bool> RemoveAsync(Expression<Func<ProductEntity, bool>> predicate)
            {
                var productToRemove = _products.AsQueryable().FirstOrDefault(predicate.Compile());
                if (productToRemove != null)
                {
                    _products.Remove(productToRemove);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
        }
    }
}
