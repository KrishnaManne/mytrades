namespace MyTrades.WebApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services){
        services.AddDbContext<MyTradesDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITradesPersistenceService, TradesPersistenceService>();
        services.AddScoped<ICapitalPersistenceService, CapitalPersistenceService>();
        return services;
    }
}