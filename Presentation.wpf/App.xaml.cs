using Business.MappingProfiles;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.wpf.ViewModels;
using System.Windows;

namespace Presentation.wpf
{
    public partial class App : Application
    {
        private static IHost? builder;

        public App()
        {
            builder = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<CustomerOrderViewModel>();

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
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            builder!.Start();
            var mainWindow = builder!.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
