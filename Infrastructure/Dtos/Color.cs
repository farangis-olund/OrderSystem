
using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class Color
{
    public string ColorName { get; set; } = null!;
       
    public static implicit operator Color(ColorEntity entity)
    {
        return new Color
        {
            ColorName = entity.ColorName

        };
    }
}
