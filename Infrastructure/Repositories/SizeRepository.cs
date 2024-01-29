
using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class SizeRepository : BaseRepository<ProductDataContext, SizeEntity>
{
    public SizeRepository(ProductDataContext context)
        : base(context)
    {

    }

}
