
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class SizeRepository : BaseRepository<ProductDataContext, SizeEntity>
{
    public SizeRepository(ProductDataContext context, ILogger<SizeRepository> logger)
        : base(context, logger)
    {

    }

}
