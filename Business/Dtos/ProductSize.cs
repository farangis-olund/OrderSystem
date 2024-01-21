
namespace Business.Dtos
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string SizeType { get; set; } = null!;
        public string SizeValue { get; set; } = null!;
        public string AgeGroup { get; set; } = null!;
    }
}
