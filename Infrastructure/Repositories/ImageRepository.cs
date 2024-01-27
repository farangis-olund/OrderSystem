
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ImageRepository : BaseRepository<ProductDataContext, ImageEntity>
{
    public ImageRepository(ProductDataContext context, ILogger<ImageRepository> logger)
        : base(context, logger)
    {

    }

}
