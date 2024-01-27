using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BrandRepository : BaseRepository<ProductDataContext, BrandEntity>
    {
        public BrandRepository(ProductDataContext context, ILogger<BrandRepository> logger)
            : base(context, logger)
        {
            
        }

    }
}
