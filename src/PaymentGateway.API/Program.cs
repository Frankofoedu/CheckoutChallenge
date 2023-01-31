using Mapster;
using PaymentGateway.API.Idempotency;
using PaymentGateway.Core.Data;
using PaymentGateway.Core.Services;
using PaymentGateway.Core.ViewModels;
using PaymentGateway.MockBank;
using Prometheus;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Host.UseSerilog((context, services, configuration)
        => configuration.ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

    builder.Services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
    builder.Services.AddScoped<IBankService, MockBankService>();

    builder.Services.RegisterDbContext();

    builder.Services.AddControllers();

    builder.Services.RegisterIdempotentConfig();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //register mapster mapping configs;
    TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(CreatePaymentResponseViewModel)));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseHttpMetrics();
    app.UseAuthorization();

    app.MapControllers();
    app.MapMetrics();
    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program


{ } //for integration tests