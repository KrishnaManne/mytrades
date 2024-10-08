using System.Net.Mime;

namespace MyTrades.WebApi;

public static class TradesEndpoints
{
    public static void MapTradesEndpoints(this WebApplication app)
    {
        const string tradesAPIGroupPath = "/api/trades";
        var trades = app.MapGroup(tradesAPIGroupPath)
                        .RequireAuthorization()
                        .WithName("Trades")
                        .WithTags("Trades")
                        .WithOpenApi();

        trades.MapGet("/{id:guid}", async (Guid id, ITradesPersistenceService tradesService) => 
            {
                try
                {
                    var result = await tradesService.GetTradeByIdAsync(id);
                    return Results.Ok(result);
                }
                catch(ApplicationException appException)
                {
                    if(appException is EntityNotFoundException)
                        return Results.NotFound();
                    throw;
                }
            })
            .Produces<TradeDto>(StatusCodes.Status200OK)
            .WithName("GetTrade");

        trades.MapGet("/paged", async (ITradesPersistenceService tradesService, int pageSize = 10, int pageNumber = 1) => 
            await tradesService.GetTradesAsync(pageSize, pageNumber))
            .Produces<PagedEntityDto<TradeDto>>(StatusCodes.Status200OK)
            .WithName("GetTrades");

        trades.MapPost("/", async (TradeDto trade, ITradesPersistenceService tradesService) =>
            {
                var createdTade = await tradesService.AddTradeAsync(trade);
                return Results.Created($"{tradesAPIGroupPath}/{createdTade.Id}", createdTade);
            })
            .Produces<TradeDto>(StatusCodes.Status201Created, contentType: "application/json")
            .Accepts<TradeDto>(contentType: "application/json")
            .WithName("PostTrade");

        trades.MapPut("/{id:guid}", async (Guid id, TradeDto trade, ITradesPersistenceService tradesService) =>
            {
                try
                {
                    await tradesService.UpdateTradeAsync(id, trade);
                    return Results.NoContent();
                }
                catch(ApplicationException appException)
                {
                    if(appException is EntityNotFoundException)
                        return Results.NotFound();
                    throw;
                }
            })
            .Produces(StatusCodes.Status204NoContent)
            .Accepts<TradeDto>(contentType: "application/json")
            .WithName("UpdateTrade");

        trades.MapDelete("/{id:guid}", async (Guid id, ITradesPersistenceService tradesService) =>
            {
                try 
                {
                    await tradesService.DeleteTradeAsync(id);
                    return Results.NoContent();
                }
                catch(ApplicationException appException)
                {
                    if(appException is EntityNotFoundException)
                        return Results.NotFound();
                    throw;
                }
            })
            .Produces(StatusCodes.Status204NoContent)
            .WithName("DeleteTrade");
    }    
}