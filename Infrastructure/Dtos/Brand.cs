

using Infrastructure.Entities;

namespace Infrastructure.Dtos
{
    public class Brand 
    {
        public string BrandName { get; set; } = null!;

        public static implicit operator Brand(BrandEntity entity)
        {
            return new Brand
            {
                BrandName = entity.BrandName

            };
        }
    }

    
}
