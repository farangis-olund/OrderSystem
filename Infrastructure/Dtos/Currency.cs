
using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class Currency
{
    public string Code { get; set; } = null!;
    public string CurrencyName { get; set; } = null!;

    public static implicit operator Currency(CurrencyEntity entity)
    {
        return new Currency
        {
            Code = entity.Code,
            CurrencyName = entity.CurrencyName

        };
    }
}
