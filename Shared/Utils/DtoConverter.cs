
namespace Shared.Utils
{
    public class DtoConverter
    {
        public IEnumerable<TDestination> ConvertEntitiesToDto<TSource, TDestination>(
        IEnumerable<TSource> entities, Func<TSource, TDestination> mappingFunction)
        {
            return entities.Select(mappingFunction);
        }

        public static TDestination ConvertEntityToDto<TSource, TDestination>(
            TSource entity, Func<TSource, TDestination> mappingFunction)
        {
            return mappingFunction(entity);
        }
    }
}
