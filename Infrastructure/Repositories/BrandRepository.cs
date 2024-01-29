using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories
{
    public class BrandRepository : BaseRepository<ProductDataContext, BrandEntity>
    {
        public BrandRepository(ProductDataContext context) : base(context)
        {
        }
    }
}
