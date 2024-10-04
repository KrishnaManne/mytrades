namespace MyTrades.WebApi;

public static class TradesEndpoints
{
    public static void MapTradesEndpoints(this WebApplication app)
    {
        var trades = app.MapGroup("/api/trades")
                        .WithName("Trades")
                        .WithTags("Trades")
                        .WithOpenApi();

        trades.MapGet("/", async (int pageSize, int pageNumber, ITradesPersistenceService tradesService) => 
            await tradesService.GetTradesAsync(pageSize, pageNumber))
        .WithName("Get Trades");

        trades.MapPost("/", async (Trade trade, ITradesPersistenceService tradesService) =>
        {
            await tradesService.AddTradeAsync(trade);
            return Results.Created();
        })
            .WithName("Post Trade");

        trades.MapPut("/{id:guid}", async (Guid id, Trade trade, ITradesPersistenceService tradesService) =>
        {
            var result = await tradesService.UpdateTradeAsync(id, trade);
            if (result is false)
                return Results.BadRequest();
            return Results.NoContent();
        })
            .WithName("Update Trade");

        trades.MapDelete("/{id:guid}", async (Guid id, ITradesPersistenceService tradesService) =>
        {
            var result = await tradesService.DeleteTradeAsync(id);
            if (result is false)
                return Results.BadRequest();
            return Results.NoContent();
        })
            .WithName("Delete Trade");
    }
}