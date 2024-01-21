using AutoMapper;
using Business.Dtos;
using Business.MappingProfiles;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    // datacontexts
    services.AddDbContext<ProductDataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\projects\OrderSystem\Infrastructure\Data\customer_database_cf.mdf;Integrated Security=True;Connect Timeout=30", x => x.MigrationsAssembly(nameof(Infrastructure))));
    services.AddDbContext<CustomerOrderContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\projects\OrderSystem\Infrastructure\Data\customer_database_cf.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True", x => x.MigrationsAssembly(nameof(Infrastructure))));

    // repositories
    // product repositories
    services.AddSingleton<BrandRepository>();
    services.AddSingleton<CategoryRepository>();
    services.AddSingleton<ProductRepository>();
    services.AddSingleton<ColorRepository>();
    services.AddSingleton<CurrencyRepository>();
    services.AddSingleton<ImageRepository>();
    services.AddSingleton<ProductImageRepository>();
    services.AddSingleton<ProductPriceRepository>();
    services.AddSingleton<ProductVariantRepository>();
    services.AddSingleton<SizeRepository>();
    // customerOrders repositories
    services.AddSingleton<CustomerRepository>();
    services.AddSingleton<CustomerOrderRepository>();
    services.AddSingleton<OrderDetailRepository>();

    // services
    // product
    services.AddSingleton<ProductService>();
    services.AddSingleton<ProductVariantService>();
    services.AddSingleton<ProductPriceService>();
    services.AddSingleton<ProductImageService>();
    services.AddSingleton<ProductImageService>();

    // customer
    services.AddSingleton<CustomerService>();
    services.AddSingleton<CustomerOrderService>();
    services.AddSingleton<OrderDetailService>();

    // mappingProfile
    services.AddAutoMapper(typeof(MappingProfile)); 

}).Build();

builder.Start();

Console.ReadKey();

//var productService = builder.Services.GetRequiredService<ProductService>();
//var result = await productService.CreateProduct(new Product
//{
//    ArticleNumber = "art1",
//    ProductName = "Test",
//    Material = "material",
//    ProductInfo = "prodInfo",
//    BrandName = "Brand",
//    CategoryName = "Category"

//});

//var productService = builder.Services.GetRequiredService<ProductVariantService>();
//var result = await productService.CreateProductVariant(new ProductVariant
//{
//    ArticleNumber = "art1",
//    Quantity = 10,
//    Price = new ProductPrice { Price = 100, CurrencyCode = "Sek", DiscountPrice = 0, DicountPercentage = 0},
//    Size = new Size { SizeType = "girls", SizeValue = "110", AgeGroup = "kids" },
//    ColorName = "Rose",
//    ImageUrls = [new() { ImageUrl = "url1" }, new() { ImageUrl = "url2" }]
//});

//var productImage = builder.Services.GetRequiredService<ProductImageService>();
//var result = await productImage.AddProductImage(new ProductImage
//{
//    ProductVariantId = 1,
//    ArticleNumber = "art1",
//    ImageUrl ="Url2"
//});

//var productPrice = builder.Services.GetRequiredService<ProductPriceService>();
//var result = await productPrice.AddProductPrice(new ProductPrice
//{
//    ProductVariantId = 1,
//    ArticleNumber = "art1",
//    Price = 10,
//    DiscountPrice = 0,
//    DicountPercentage = 0,
//    Code = "Sek",
//    CurrencyName = "Swedish Krona"

//});

//var customer = builder.Services.GetRequiredService<CustomerService>();
//var result = await customer.AddCustomer(new Customer
//{
//    FirstName = "Farangis",
//    LastName = "Ölund",
//    Email = "bless.faro@gmail.com",
//    PhoneNumber = "214235435"
//});


var serviceProvider = builder.Services;
//var customerService = builder.Services.GetRequiredService<CustomerService>();
//var customerOrderService = builder.Services.GetRequiredService<CustomerOrderService>();
var mapper = serviceProvider.GetRequiredService<IMapper>();

//var existingCustomer =  mapper.Map<Customer>(await customerService.GetCustomer("bless.faro@gmail.com"));
//var result = await customerOrderService.AddCustomerOrder(new CustomerOrder
//{
//    TotalAmount = 10,
//    Date = DateOnly.FromDateTime(DateTime.Now),
//    Customer = existingCustomer
//});

var orderDetailService = builder.Services.GetRequiredService<OrderDetailService>();
var productVariantService = builder.Services.GetRequiredService<ProductVariantService>();
var customerOrderService = builder.Services.GetRequiredService<CustomerOrderService>();

var existingProductVariant =  mapper.Map<ProductVariant>(await productVariantService.GetProductVariantById(1));
var existingCustomerOrder = mapper.Map<CustomerOrder>(await customerOrderService.GetCustomerOrderById(1));

var result = await orderDetailService.AddOrderDetail(new OrderDetail
{
    CustomerOrder = existingCustomerOrder,
    ProductVariant = existingProductVariant,
    Quantity = 1

});

if (result != null)
{
    Console.WriteLine("Order added successfully!");
}
else
{
    Console.WriteLine("Failed to add order detail.");
}

Console.ReadKey();