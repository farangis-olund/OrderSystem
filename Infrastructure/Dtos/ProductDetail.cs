
using System.Drawing;

namespace Infrastructure.Dtos
{
    public class ProductDetail
    {
        public string ArticleNumber { get; set; } = null!;
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } = null!;
        public string Material { get; set; } = null!;
        public string ProductInfo { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public int BrandId { get; set; } 
        public string ImageUrl { get; set; } = null!;
        public string ColorName { get; set; } = null!;
        public string SizeValue { get; set; } = null!;
        public int SizeId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public int CategoryId { get; set; }
        public ICollection<Size> Size { get; set; } = new List<Size>();

        
        public static implicit operator ProductDetail(Product product)
        {
            return new ProductDetail
            {
                ArticleNumber = product.ArticleNumber,
                ProductName = product.ProductName,
                Material = product.Material,
                ProductInfo = product.ProductInfo,
                BrandName = product.BrandName,
                CategoryName = product.CategoryName,
                CategoryId = product.CategoryId

            };
        }


    }
}
