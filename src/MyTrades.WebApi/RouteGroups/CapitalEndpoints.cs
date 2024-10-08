namespace MyTrades.WebApi;

public static class CapitalEndpoints
{
    public static void MapCapitalEndpoints(this WebApplication app)
    {
        const string CapitalApiGroupPath = "api/capital";
        var group = app.MapGroup(CapitalApiGroupPath)
                        .RequireAuthorization()
                        .WithName("Capital")
                        .WithTags("Capital");
        
        group.MapGet("/{id:guid}", async (Guid id, ICapitalPersistenceService capitalPersistenceService) => 
            {
                try
                {
                    var result = await capitalPersistenceService.GetCapitalAsync(id);
                    return Results.Ok(result);
                }
                catch(ApplicationException appException)
                {
                    if(appException is EntityNotFoundException)
                        return Results.NotFound();
                    throw;
                }
            })  
            .Produces<CapitalDto>(StatusCodes.Status200OK)
            .WithName("GetCapital");

        group.MapPost("/", async (CapitalDto capital, ICapitalPersistenceService capitalPersistenceService) =>
            {            
                var createdCapital = await capitalPersistenceService.AddCapitalAsync(capital);
                return Results.Created($"{CapitalApiGroupPath}/{createdCapital.Id}", createdCapital);
            })
            .Accepts<CapitalDto>(contentType: "application/json")
            .Produces<CapitalDto>(StatusCodes.Status201Created)
            .WithName("PostCapital");

        group.MapPut("/{id:guid}", async (Guid id, CapitalDto capital, ICapitalPersistenceService capitalPersistenceService) =>
            {
                try
                {
                    await capitalPersistenceService.UpdateCapitalAsync(id, capital);
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
            .WithName("UpdateCapital");

        group.MapDelete("/{id:guid}", async (Guid id, ICapitalPersistenceService capitalPersistenceService) =>
            {
                try
                {
                    await capitalPersistenceService.DeleteCapitalAsync(id);
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
            .WithName("DeleteCapital");
    }
}