
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class ColorRepository : BaseRepository<ProductDataContext, ColorEntity>
    {
        public ColorRepository(ProductDataContext context, ILogger<ColorRepository> logger)
            : base(context, logger)
        {

        }

    }
}
