using Infrastructure.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.wpf.ViewModels;
using Presentation.wpf.Views;
using System.Windows;
using System.Collections.ObjectModel;
using Infrastructure.Dtos;

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
                    // presentation services

                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<CustomerOrderViewModel>();
                    services.AddSingleton<CustomerOrderView>();
                    services.AddSingleton<ProductListViewModel>();
                    services.AddSingleton<ProductListView>();
                    services.AddSingleton<ObservableCollection<Product>>();

                    // datacontexts

                    services.AddDbContext<ProductDataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\projects\OrderSystem\Infrastructure\Data\customer_database_cf.mdf;Integrated Security=True;Connect Timeout=30", x => x.MigrationsAssembly(nameof(Infrastructure))));
                    services.AddDbContext<CustomerOrderContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\projects\OrderSystem\Infrastructure\Data\customer_database_cf.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True", x => x.MigrationsAssembly(nameof(Infrastructure))));

                    // repositories
                    
                    // product repositories
                    services.AddScoped<BrandRepository>();
                    services.AddScoped<CategoryRepository>();
                    services.AddScoped<ProductRepository>();
                    services.AddScoped<ColorRepository>();
                    services.AddScoped<CurrencyRepository>();
                    services.AddScoped<ImageRepository>();
                    services.AddScoped<ProductImageRepository>();
                    services.AddScoped<ProductPriceRepository>();
                    services.AddScoped<ProductVariantRepository>();
                    services.AddScoped<SizeRepository>();
                    services.AddSingleton<Product>();

                    // customerOrders repositories
                    services.AddScoped<CustomerRepository>();
                    services.AddScoped<CustomerOrderRepository>();
                    services.AddScoped<OrderDetailRepository>();

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
