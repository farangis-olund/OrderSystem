
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class ProductPresentationService
    {
        private readonly BrandRepository _brandRepository;
        private readonly ProductCategoryService _categoryService;
        private readonly ProductRepository _productRepository;
        private readonly ProductVariantService _productVariantService;
        private readonly ProductImageService _productImageService;
        private readonly ImageRepository _imageRepository;
        private readonly ProductPriceService _productPriceService;
        private readonly ProductService _productService;
        private readonly ColorRepository _colorRepository;
        private readonly ProductSizeService _productSizeService;
        private readonly ILogger<ProductService> _logger;

        public ProductPresentationService(BrandRepository brandRepository,
            ProductCategoryService categoryService,
            ProductRepository productRepository,
            ProductVariantService productVariantService,
            ProductImageService productImageService,
            ImageRepository imageRepository,
            ProductPriceService productPriceService,
            ColorRepository colorRepository,
            ProductService productService,
            ProductSizeService productSizeService,
            ILogger<ProductService> logger)
        {
            _brandRepository = brandRepository;
            _categoryService = categoryService;
            _productRepository = productRepository;
            _productVariantService = productVariantService;
            _productImageService = productImageService;
            _imageRepository = imageRepository;
            _productPriceService = productPriceService;
            _colorRepository = colorRepository;
            _productService = productService;
            _productSizeService = productSizeService;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDetail>> GetAllProductDetailsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();

                var productsWithImages = new List<ProductDetail>();

                foreach (var product in products)
                {
                    var (imageUrl, colorName) = await GetProductImageUrlAsync(product.ArticleNumber);
                    var (price, currency) = await GetProductPriceAsync(product.ArticleNumber);
                    var productVariant = await _productVariantService.GetProductVariantByArticleNumberAsync(product.ArticleNumber);
                    var category = await _categoryService.GetCategoryAsync(product.CategoryId);
                    var brand = await _brandRepository.GetOneAsync(b => b.Id == product.BrandId);
                    var size = await _productSizeService.GetSizeAsync(productVariant.SizeId);
                    
                    var productWithImage = new ProductDetail
                    {
                        ArticleNumber = product.ArticleNumber,
                        ProductVariantId = productVariant?.Id ?? 0,
                        ProductName = product.ProductName,
                        ImageUrl = imageUrl,
                        Price = price,
                        Currency = currency,
                        ProductInfo = product.ProductInfo!,
                        Material = product.Material!,
                        ColorName = colorName,
                        CategoryId = category.Id,
                        CategoryName = category.CategoryName,
                        BrandName = brand.BrandName,
                        BrandId = brand.Id,
                        SizeId = productVariant?.SizeId ?? 0,
                        SizeValue = size.SizeValue!

                    };

                    productsWithImages.Add(productWithImage);
                }

                return productsWithImages;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in fetching products with images: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return Enumerable.Empty<ProductDetail>();
            }
        }

        private async Task<(string ImageUrl, string ColorName)> GetProductImageUrlAsync(string articleNumber)
        {
            try
            {
                var productVariant = await _productVariantService.GetProductVariantByArticleNumberAsync(articleNumber);

                if (productVariant == null)
                {
                    return (null, null)!;
                }
                var color = await _colorRepository.GetOneAsync(c => c.Id == productVariant.ColorId);
                var productImage = await _productImageService.GetProductImageAsync(productVariant.Id);

                if (productImage == null)
                {
                    return (null, null)!;
                }
                var image = await _imageRepository.GetOneAsync(i => i.Id == productImage.ImageId);
                return (image.ImageUrl, color.ColorName);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in fetching products with images: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return (null, null)!;
            }
            
        }

        private async Task<(decimal Price, string CurrencyCode)> GetProductPriceAsync(string articleNumber)
        {
            try
            {
                var productVariant = await _productVariantService.GetProductVariantByArticleNumberAsync(articleNumber);

                if (productVariant != null)
                {
                    var productPrice = await _productPriceService.GetProductPriceByProductVariantAsync(productVariant.Id, productVariant.ArticleNumber);

                    if (productPrice != null)
                    {
                        return (productPrice.Price, productPrice.CurrencyCode);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in fetching products with images: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return (0, null)!;
            }
            

            return (0, null!);
        }

        public async Task<bool> UpdateProductDetailAsync(ProductDetail productDetail)
        {
            try
            {
                if (productDetail != null)
                {
                    
                    await _productService.UpdateProductAsync(productDetail);
                    await _productVariantService.UpdateProductVariantAsync(productDetail);
                    await _categoryService.UpdateCategoryAsync(productDetail.CategoryId, productDetail.CategoryName);
                    await _productPriceService.UpdateProductPriceByProductVariantAsync(productDetail.ProductVariantId, productDetail.ArticleNumber, productDetail.Price);
                    await _productImageService.UpdateProductImageAsync(productDetail.ProductVariantId, productDetail.ArticleNumber, productDetail.ImageUrl);
                    
                    return true;
                }
                else 
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in fetching products with images: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return false;
            }

           


        }
    }
}
