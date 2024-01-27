
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
        private readonly CategoryService _categoryService;
        private readonly ProductRepository _productRepository;
        private readonly ProductVariantService _productVariantService;
        private readonly ProductImageService _productImageService;
        private readonly ImageRepository _imageRepository;
        private readonly PriceService _productPriceService;
        private readonly ProductService _productService;
        private readonly ColorRepository _colorRepository;
        private readonly SizeService _productSizeService;
        private readonly ILogger<ProductService> _logger;

        public ProductPresentationService(BrandRepository brandRepository,
            CategoryService categoryService,
            ProductRepository productRepository,
            ProductVariantService productVariantService,
            ProductImageService productImageService,
            ImageRepository imageRepository,
            PriceService productPriceService,
            ColorRepository colorRepository,
            ProductService productService,
            SizeService productSizeService,
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


        //public async Task<bool> UpdateProductDetailAsync(ProductDetail productDetail)
        //{
        //    try
        //    {
        //        if (productDetail != null)
        //        {

        //            await _productService.UpdateProductAsync(productDetail);
        //            await _productVariantService.UpdateProductVariantAsync(productDetail);
        //            await _categoryService.UpdateCategoryAsync(productDetail.CategoryId, productDetail.CategoryName);
        //            await _productPriceService.UpdateProductPriceByProductVariantAsync(productDetail.ProductVariantId, productDetail.ArticleNumber, productDetail.Price);
        //            await _productImageService.UpdateProductImageAsync(productDetail.ProductVariantId, productDetail.ArticleNumber, productDetail.ImageUrl);

        //            return true;
        //        }
        //        else
        //            return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error in fetching products with images: {ex.Message}");
        //        Debug.WriteLine(ex.Message);
        //        return false;
        //    }




        //}
    }
}
