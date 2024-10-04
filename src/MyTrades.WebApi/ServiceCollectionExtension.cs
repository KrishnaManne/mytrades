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
        })
        .WithLogging();
        return services;
    }
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services){
        services.AddDbContext<MyTradesDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITradesPersistenceService, TradesPersistenceService>();
        services.AddScoped<ICapitalPersistenceService, CapitalPersistenceService>();
        return services;
    }
}