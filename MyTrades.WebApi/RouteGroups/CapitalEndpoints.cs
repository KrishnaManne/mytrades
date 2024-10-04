namespace MyTrades.WebApi;

public static class CapitalEndpoints
{
    public static void MapCapitalEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/capital")
                        .WithName("Capital")
                        .WithTags("Capital");
        
        group.MapGet("/", async (ICapitalPersistenceService capitalPersistenceServiceService) => 
        {
            var capital = await capitalPersistenceServiceService.GetCapitalAsync();
            return capital is null ? Results.NotFound() : Results.Ok(capital);
        })  
        .WithName("Get Capital");

        group.MapPost("/", async (Capital capital, ICapitalPersistenceService capitalPersistenceServiceService) =>
        {
            await capitalPersistenceServiceService.AddCapitalAsync(capital);
            return Results.Created();
        })
        .WithName("Post Capital");

        group.MapPut("/{id:guid}", async (Guid id, Capital capital, ICapitalPersistenceService capitalPersistenceServiceService) =>
        {
            var result = await capitalPersistenceServiceService.UpdateCapitalAsync(id, capital);
            if (result is false)
                return Results.BadRequest();
            return Results.NoContent();
        })
        .WithName("Update Capital");

        group.MapDelete("/{id:guid}", async (Guid id, ICapitalPersistenceService capitalPersistenceServiceService) =>
        {
            var result = await capitalPersistenceServiceService.DeleteCapitalAsync(id);
            if (result is false)
                return Results.BadRequest();
            return Results.NoContent();
        })
        .WithName("Delete Capital");
    }
}