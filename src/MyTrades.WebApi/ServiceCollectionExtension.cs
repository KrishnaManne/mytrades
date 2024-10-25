using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MyTrades.WebApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenTelemetryConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
        .ConfigureResource(r => 
            r.AddService(serviceName: configuration.GetValue("ServiceName", defaultValue: "otel-test")!,
            serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
            serviceInstanceId: Environment.MachineName))
        .UseOtlpExporter(OtlpExportProtocol.HttpProtobuf, 
            new Uri(configuration.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:18890")!))
        .WithMetrics(metrics =>
        {   
            metrics.AddAspNetCoreInstrumentation();
            metrics.AddHttpClientInstrumentation();
        })
        .WithTracing(tracing => 
        {
            tracing.AddHttpClientInstrumentation();
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddEntityFrameworkCoreInstrumentation();
        })
        .WithLogging();
        return services;
    }
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration){
        services.AddDbContext<MyTradesDbContext>(options =>{
            options.UseSqlServer(configuration.GetConnectionString("AzureSqlDatabase"));
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITradesPersistenceService, TradesPersistenceService>();
        services.AddScoped<ICapitalPersistenceService, CapitalPersistenceService>();
        services.AddScoped<IUserPersistenceService, UserPersistenceService>();
        services.AddScoped<IOTPPersistenceService, OTPPersistenceService>();
        services.AddScoped<ISignInManagementService, SignInManagementService>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
}