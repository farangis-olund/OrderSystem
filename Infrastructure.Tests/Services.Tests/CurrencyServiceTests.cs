
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
    public class CurrencyServiceTests
    {
        private readonly InMemoryCurrencyRepository _inMemoryCurrencyRepository;
        private readonly Mock<ILogger<CurrencyService>> _mockLogger;

        public CurrencyServiceTests()
        {
            var context = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _inMemoryCurrencyRepository = new InMemoryCurrencyRepository(context);
            _mockLogger = new Mock<ILogger<CurrencyService>>();
        }

        [Fact]
        public async Task AddCurrencyAsync_ShouldAddCurrency_WhenCurrencyDoesNotExist()
        {
            // Arrange
            var currencyService = new CurrencyService(_inMemoryCurrencyRepository, _mockLogger.Object);
            var currencyCode = "USD";
            var currencyName = "US Dollar";

            // Act
            var result = await currencyService.AddCurrencyAsync(currencyCode, currencyName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(currencyCode, result.Code);
            Assert.Equal(currencyName, result.CurrencyName);
        }

        [Fact]
        public async Task AddCurrencyAsync_ShouldReturnExistingCurrency_WhenCurrencyExists()
        {
            // Arrange
            var currencyService = new CurrencyService(_inMemoryCurrencyRepository, _mockLogger.Object);
            var currencyCode = "USD";
            var currencyName = "US Dollar";
            var existingCurrency = new CurrencyEntity { Code = currencyCode, CurrencyName = currencyName };
            await _inMemoryCurrencyRepository.AddAsync(existingCurrency);

            // Act
            var result = await currencyService.AddCurrencyAsync(currencyCode, currencyName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingCurrency, result);
        }

        [Fact]
        public async Task GetCurrencyAsync_ShouldReturnCurrency_WhenCurrencyExists()
        {
            // Arrange
            var currencyService = new CurrencyService(_inMemoryCurrencyRepository, _mockLogger.Object);
            var currencyCode = "USD";
            var currencyName = "US Dollar";
            var existingCurrency = new CurrencyEntity { Code = currencyCode, CurrencyName = currencyName };
            await _inMemoryCurrencyRepository.AddAsync(existingCurrency);

            // Act
            var result = await currencyService.GetCurrencyAsync(currencyCode);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingCurrency, result);
        }

        [Fact]
        public async Task GetCurrencyAsync_ShouldReturnNull_WhenCurrencyDoesNotExist()
        {
            // Arrange
            var currencyService = new CurrencyService(_inMemoryCurrencyRepository, _mockLogger.Object);
            var currencyCode = "EUR"; // Non-existent currency code

            // Act
            var result = await currencyService.GetCurrencyAsync(currencyCode);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCurrencyAsync_ShouldUpdateCurrency_WhenCurrencyExists()
        {
            // Arrange
            var currencyService = new CurrencyService(_inMemoryCurrencyRepository, _mockLogger.Object);
            var currencyCode = "USD";
            var currencyName = "US Dollar";
            var existingCurrency = new CurrencyEntity { Code = currencyCode, CurrencyName = currencyName };
            await _inMemoryCurrencyRepository.AddAsync(existingCurrency);

            // Act
            existingCurrency.CurrencyName = "Updated US Dollar";
            var result = await currencyService.UpdateCurrencyAsync(existingCurrency);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated US Dollar", result.CurrencyName);
        }

        [Fact]
        public async Task GetAllCurrenciesAsync_ShouldReturnAllCurrencies_WhenCurrenciesExist()
        {
            // Arrange
            var currencyService = new CurrencyService(_inMemoryCurrencyRepository, _mockLogger.Object);
            var testData = new List<CurrencyEntity>
            {
                new() { Code = "USD", CurrencyName = "US Dollar" },
                new() { Code = "EUR", CurrencyName = "Euro" }
            };

            foreach (var currency in testData)
            {
                await _inMemoryCurrencyRepository.AddAsync(currency);
            }

            // Act
            var result = await currencyService.GetAllCurrenciesAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(testData.Count, result.Count());
        }

        [Fact]
        public async Task DeleteCurrencyAsync_ShouldDeleteCurrency_WhenCurrencyExists()
        {
            // Arrange
            var currencyService = new CurrencyService(_inMemoryCurrencyRepository, _mockLogger.Object);
            var currencyCode = "USD";
            var currencyName = "US Dollar";
            var currencyToDelete = new CurrencyEntity { Code = currencyCode, CurrencyName = currencyName };
            await _inMemoryCurrencyRepository.AddAsync(currencyToDelete);

            // Act
            var result = await currencyService.DeleteCurrencyAsync(currencyCode);

            // Assert
            Assert.True(result);

            var deletedCurrency = await _inMemoryCurrencyRepository.GetOneAsync(c => c.Code == currencyCode);
            Assert.Null(deletedCurrency);
        }

        [Fact]
        public async Task DeleteCurrencyAsync_ShouldNotDeleteCurrency_WhenCurrencyDoesNotExist()
        {
            // Arrange
            var currencyService = new CurrencyService(_inMemoryCurrencyRepository, _mockLogger.Object);
            var currencyCode = "EUR"; 

            // Act
            var result = await currencyService.DeleteCurrencyAsync(currencyCode);

            // Assert
            Assert.False(result);
        }
    }

    public class InMemoryCurrencyRepository : CurrencyRepository
    {
        private readonly List<CurrencyEntity> _currencies;

        public InMemoryCurrencyRepository(ProductDataContext context) : base(context)
        {
            _currencies = new List<CurrencyEntity>();
        }

        public override Task<IEnumerable<CurrencyEntity>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<CurrencyEntity>>(_currencies);
        }

        public override Task<CurrencyEntity> GetOneAsync(Expression<Func<CurrencyEntity, bool>> predicate)
        {
            return Task.FromResult(_currencies.AsQueryable().FirstOrDefault(predicate.Compile())!);
        }

        public override Task<CurrencyEntity> AddAsync(CurrencyEntity entity)
        {
            _currencies.Add(entity);
            return Task.FromResult(entity);
        }

        public override Task<CurrencyEntity> UpdateAsync(Expression<Func<CurrencyEntity, bool>> predicate, CurrencyEntity entity)
        {
            var existingCurrency = _currencies.AsQueryable().FirstOrDefault(predicate.Compile());
            if (existingCurrency != null)
            {
                existingCurrency.Code = entity.Code;
                existingCurrency.CurrencyName = entity.CurrencyName;
            }
            return Task.FromResult(existingCurrency!);
        }

        public override Task<bool> RemoveAsync(Expression<Func<CurrencyEntity, bool>> predicate)
        {
            var entityToRemove = _currencies.FirstOrDefault(predicate.Compile());
            if (entityToRemove != null)
            {
                _currencies.Remove(entityToRemove);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
