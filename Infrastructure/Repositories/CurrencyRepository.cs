﻿
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class CurrencyRepository : BaseRepository<ProductDataContext, CurrencyEntity>
    {
        public CurrencyRepository(ProductDataContext context, ILogger<CurrencyRepository> logger)
            : base(context, logger)
        {

        }

        public override Task<CurrencyEntity> AddAsync(CurrencyEntity entity)
        {
            return base.AddAsync(entity);
        }

        public override Task<bool> Exist(Expression<Func<CurrencyEntity, bool>> predicate)
        {
            return base.Exist(predicate);
        }

        public override Task<IEnumerable<CurrencyEntity>> GetAllAsync()
        {
            return base.GetAllAsync();
        }

        public override Task<CurrencyEntity> GetOneAsync(Expression<Func<CurrencyEntity, bool>> predicate)
        {
            return base.GetOneAsync(predicate);
        }

        public override Task<bool> RemoveAsync(Expression<Func<CurrencyEntity, bool>> predicate)
        {
            return base.RemoveAsync(predicate);
        }

        public override Task<bool> UpdateAsync(CurrencyEntity entity)
        {
            return base.UpdateAsync(entity);
        }
    }
}