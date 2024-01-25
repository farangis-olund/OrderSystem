
using Infrastructure.Entities;

namespace Infrastructure.Dtos
{
    public class ProductSize
    {
        public string SizeType { get; set; } = null!;
        public string? SizeValue { get; set; } = null!;
        public string? AgeGroup { get; set; } = null!;

        public static implicit operator ProductSize(SizeEntity entity)
        {
            return new ProductSize
            {
                SizeType = entity.SizeType ?? default!,
                SizeValue = entity.SizeValue!,
                AgeGroup = entity.AgeGroup!

            };
        }
    }
}
