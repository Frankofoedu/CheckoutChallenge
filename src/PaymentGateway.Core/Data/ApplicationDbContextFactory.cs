using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Core.DataAccess;

namespace PaymentGateway.Core.Data
{
    public static class ApplicationDbContextFactory
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetService<IConfiguration>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("PostgresConnection"),
                    x =>
                    {
                        x.MigrationsAssembly("PaymentGateway.Core");
                    })
                );

            //register repository
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            return services;
        }
    }
}