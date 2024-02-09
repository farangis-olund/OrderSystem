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
using Presentation.wpf.Services;
using Shared.Utils;

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
                    services.AddTransient<ProductListViewModel>();
                    services.AddTransient<ProductListView>();
                    services.AddTransient<AddProductViewModel>();
                    services.AddTransient<AddProductView>();
                    services.AddTransient<ProductUpdateViewModel>();
                    services.AddTransient<ProductUpdateView>();
                    services.AddTransient<CustomerListView>();
                    services.AddTransient<CustomerListViewModel>();
                    services.AddTransient<AddCustomerView>();
                    services.AddTransient<AddCustomerViewModel>();
                    services.AddTransient<UpdateCustomerView>();
                    services.AddTransient<UpdateCustomerViewModel>();
                    services.AddTransient<OrderListView>();
                    services.AddTransient<OrderListViewModel>();
                    services.AddTransient<OrderDetailsViewModel>();
                    services.AddTransient<OrderDetailsView>();
                    services.AddTransient<AddOrderViewModel>();
                    services.AddTransient<AddOrderView>();
                    
                    // Dtos transferService
                    services.AddScoped<DataTransferService>();
                    services.AddScoped<DtoConverter>();

                    // ObservableColection
                    services.AddTransient<ObservableCollection<ProductDetail>>();
                    services.AddTransient<ObservableCollection<CustomerOrder>>();
                    services.AddTransient<ObservableCollection<ProductSize>>();
                    services.AddTransient<ObservableCollection<Currency>>();
                    services.AddTransient<ObservableCollection<Customer>>();

                    // datacontexts

                    services.AddDbContext<ProductDataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\projects\OrderSystem\Infrastructure\Data\productCatalog_database_df.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True", x => x.MigrationsAssembly(nameof(Infrastructure))));
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
                    services.AddScoped<PriceRepository>();
                    services.AddScoped<ProductVariantRepository>();
                    services.AddScoped<SizeRepository>();

                    services.AddSingleton<Product>();
                    services.AddSingleton<ProductVariant>();
                    services.AddSingleton<ProductDetail>();
                    services.AddSingleton<ProductSize>();
                    services.AddSingleton<Currency>();
                    services.AddSingleton<Customer>();

                    // customerOrders repositories
                    services.AddScoped<CustomerRepository>();
                    services.AddScoped<CustomerOrderRepository>();
                    services.AddScoped<OrderDetailRepository>();

                    // services

                    // product
                    services.AddScoped<BrandService>();
                    services.AddScoped<CategoryService>();
                    services.AddScoped<ColorService>();
                    services.AddScoped<CurrencyService>();
                    services.AddScoped<ImageService>();
                    services.AddScoped<PriceService>();
                    services.AddScoped<ProductImageService>();
                    services.AddScoped<ProductVariantService>();
                    services.AddScoped<ProductService>();
                    services.AddScoped<ProductVariantService>();
                    services.AddScoped<SizeService>();
                    
                    // customer
                    services.AddScoped<CustomerService>();
                    services.AddScoped<CustomerOrderService>();
                    services.AddScoped<OrderDetailService>();


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
