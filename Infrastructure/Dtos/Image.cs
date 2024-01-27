
using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class Image
{
    public string ImageUrl { get; set; } = null!;

    public static implicit operator Image(ImageEntity entity)
    {
        return new Image
        {
            ImageUrl = entity.ImageUrl
        };
    }

}
