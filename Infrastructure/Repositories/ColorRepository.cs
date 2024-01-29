
using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories
{
    public class ColorRepository : BaseRepository<ProductDataContext, ColorEntity>
    {
        public ColorRepository(ProductDataContext context)
            : base(context)
        {

        }

    }
}
