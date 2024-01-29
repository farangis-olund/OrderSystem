
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Repositories
{
    public class CurrencyRepository : BaseRepository<ProductDataContext, CurrencyEntity>
    {
        public CurrencyRepository(ProductDataContext context)
            : base(context)
        {

        }
    }
}
