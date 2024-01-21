using Microsoft.Extensions.Hosting;

IHost? builder;


    builder = Host.CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            services.AddDbContext<CustomerOrderContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\projects\OrderSystem\Infrastructure\Data\customer_database_cf.mdf;Integrated Security=True;Connect Timeout=30"));

        }).Build();
