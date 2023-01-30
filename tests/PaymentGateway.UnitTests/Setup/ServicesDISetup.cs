using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Core.Data;
using PaymentGateway.Core.DataAccess;
using PaymentGateway.Core.Services;
using PaymentGateway.MockBank;

namespace PaymentGateway.UnitTests.Setup;

public class ServicesDISetup : IDisposable
{
    public ServiceCollection Services { get; private set; }
    public ServiceProvider ServiceProvider { get; protected set; }
    public ApplicationDbContext DbContext { get; set; }

    public ServicesDISetup()
    {
        Services = new ServiceCollection();

        var guid = Guid.NewGuid().ToString().Replace("-", "");
        Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql($"Server=localhost;Port=1434;Database=test.{guid};User Id=postgres;Password=root"));

        Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

        Services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
        Services.AddScoped<IBankService, MockBankService>();

        ServiceProvider = Services.AddLogging().BuildServiceProvider();

        DbContext = ServiceProvider.GetRequiredService<ApplicationDbContext>();
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}