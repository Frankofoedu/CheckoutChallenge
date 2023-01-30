using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Core.Data;
using Xunit;

namespace PaymentGateway.IntegrationTests
{
    [CollectionDefinition("SetupDatabase")]
    public class SetupDatabase : IDisposable, ICollectionFixture<SetupDatabase>
    {
        public ApplicationDbContext DbContext { get; set; }

        public SetupDatabase()
        {
            var services = new ServiceCollection();
            ServiceProvider ServiceProvider;
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql($"Server=localhost;Port=1433;Database=payment;User Id=postgres;Password=root"));

            ServiceProvider = services.AddLogging().BuildServiceProvider();

            DbContext = ServiceProvider.GetService<ApplicationDbContext>();

            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }
    }
}