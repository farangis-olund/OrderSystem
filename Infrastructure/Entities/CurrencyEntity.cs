using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index("CurrencyName", Name = "UQ__Currency__3D13D298E3C8E079", IsUnique = true)]
public partial class CurrencyEntity
{
    [Key]
    [StringLength(3)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(20)]
    public string CurrencyName { get; set; } = null!;

    [InverseProperty("CurrencyCodeNavigation")]
    public virtual ICollection<ProductPriceEntity> ProductPriceEntities { get; set; } = new List<ProductPriceEntity>();
}
